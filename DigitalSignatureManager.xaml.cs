using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.Numerics;

using SautinSoft.Document;
using SautinSoft.Document.Drawing;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для DigitalSignatureManager.xaml
    /// </summary>
    public partial class DigitalSignatureManager : Window
    {
        public class DigitalSignatureFile
        {
            public string signed_by { get; set; } = string.Empty;
            public string file_name { get; set; } = string.Empty;
            public string hash { get; set; } = string.Empty;
            public string cipher { get; set; } = string.Empty;
            public string closed_exponent { get; set; } = string.Empty;
            public string multiplication { get; set; } = string.Empty;
        }

        public class DigitalSignatureData
        {
            public string user { get; set; } = string.Empty;
            public string open_exponent { get; set; } = string.Empty;
            public string closed_exponent { get; set; } = string.Empty;
            public string multiplication { get; set; } = string.Empty;
            public string file_name { get; set; } = string.Empty;
            public string hash { get; set; } = string.Empty;
            public string cipher { get; set; } = string.Empty;
        }

        public List<DigitalSignatureData> Database { get; set; }
        public List<DigitalSignatureData> CurrentView { get; set; }
        private RSA rsa;

        public DigitalSignatureManager()
        {
            Database = JsonSerializer.Deserialize<List<DigitalSignatureData>>(File.ReadAllText(ProgramConstants.DIGITAL_SIGNATURE_JSON));

            if (Database == null) return;

            CurrentView = Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root" ?
                          Database.AsParallel().Where(x => x.user == Authentication.CurrentUser).ToList() :
                          Database;

            InitializeComponent();

            dgData.ItemsSource = CurrentView;

            if (Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root")
                clmLogin.Visibility = Visibility.Hidden;
        }

        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            rsa = new RSA();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые документы (*.docx, *.doc, *.txt)|*.docx;*.doc;*.txt|Все файлы (*.*)|*.*";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();
            
            if (openFileDialog.FileName == null || openFileDialog.FileName == string.Empty)
                return;

            txtFileName.Text = openFileDialog.FileName;

            btnSaveToTable.Visibility = Visibility.Visible;
        }

        private void btnSaveToTable_Click(object sender, RoutedEventArgs e)
        {
            if (IsTextBoxEmpty(ref txtFileName))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_SAVE_DESCRIPTION, ProgramConstants.DS_ERROR_HEADER);
                return;
            }

            DocumentCore dc = DocumentCore.Load(txtFileName.Text);
            DocumentPaginator dp = dc.GetPaginator();

            Picture pic = new Picture(dc, ProgramConstants.DIGITAL_SIGNATURE_PNG);
            dp.Pages[dp.Pages.Count - 1].Content.End.Insert(pic.Content);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые документы (*.pdf)|*.pdf";
            saveFileDialog.FileName = txtFileName.Text + ".pdf";
            
            if (saveFileDialog.ShowDialog() == true)
                dc.Save(saveFileDialog.FileName);
            else
                return;

            txtHash.Text = Convert.ToHexString(new Streebog().ComputeHash(File.ReadAllBytes(txtFileName.Text)));
            txtCipher.Text = rsa.Encrypt(txtHash.Text);

            BigInteger openExp, closedExp, mult;
            (openExp, mult) = rsa.GetPublicKey();
            (closedExp, mult) = rsa.GetPrivateKey();

            var row = new DigitalSignatureData
            {
                user            = Authentication.CurrentUser,
                open_exponent   = openExp.ToString(),
                closed_exponent = closedExp.ToString(),
                multiplication  = mult.ToString(),
                file_name       = txtFileName.Text,
                hash            = txtHash.Text,
                cipher          = txtCipher.Text
            };

            CurrentView.Add(row);
            dgData.Items.Refresh();

            var sigdata = new DigitalSignatureFile
            {
                signed_by       = Authentication.CurrentUser,
                file_name       = saveFileDialog.FileName,
                hash            = txtHash.Text,
                cipher          = txtCipher.Text,
                closed_exponent = closedExp.ToString(),
                multiplication  = mult.ToString()
            };

            File.WriteAllText(saveFileDialog.FileName + ".sig", JsonSerializer.Serialize(sigdata, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root")
                Database = Database.AsParallel().Where(x => x.user != Authentication.CurrentUser).ToList().Concat(CurrentView).ToList();
            else
                Database = CurrentView;

            File.WriteAllText(ProgramConstants.DIGITAL_SIGNATURE_JSON, JsonSerializer.Serialize(Database, new JsonSerializerOptions { WriteIndented = true }));

            Close(); 
        }

        private bool IsTextBoxEmpty(ref TextBox textBox) => textBox.Text == string.Empty || textBox.Text == textBox.ToolTip.ToString() ? true : false;
    }
}
