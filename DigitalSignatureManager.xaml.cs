using Microsoft.Win32;
using System.IO;
using SautinSoft.Document;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.Numerics;
using System.Security.Cryptography;


namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для DigitalSignatureManager.xaml
    /// </summary>
    public partial class DigitalSignatureManager : Window
    {
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
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();
            
            if (openFileDialog.FileName == null || openFileDialog.FileName == string.Empty)
                return;

            txtFileName.Text = openFileDialog.FileName;
            txtHash.Text = Convert.ToHexString(new Streebog().ComputeHash(File.ReadAllBytes(txtFileName.Text)));
            txtCipher.Text = rsa.Encrypt(txtHash.Text);

            btnSaveToTable.Visibility = Visibility.Visible;

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.ShowDialog();
            //if (saveFileDialog.ShowDialog() == true)
            //File.WriteAllText(saveFileDialog.FileName, JsonSerializer.Serialize(Database, new JsonSerializerOptions { WriteIndented = true }));

            //DocumentCore dc = DocumentCore.Load(txtFileName.Text);
            //foreach (Run run in dc.GetChildElements(true, ElementType.Run))
            //{
            //    MessageBox.Show(run.Text);
            //}
        }

        private void btnSaveToTable_Click(object sender, RoutedEventArgs e)
        {
            if (IsTextBoxEmpty(ref txtHash) || IsTextBoxEmpty(ref txtCipher))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_SAVE_DESCRIPTION, ProgramConstants.DS_ERROR_HEADER);
                return;
            }

            BigInteger openExp, closedExp, mult;
            (openExp, mult) = rsa.GetPublicKey();
            (closedExp, mult) = rsa.GetPrivateKey();

            var data = new DigitalSignatureData
            {
                user            = Authentication.CurrentUser,
                open_exponent   = openExp.ToString(),
                closed_exponent = closedExp.ToString(),
                multiplication  = mult.ToString(),
                file_name       = txtFileName.Text,
                hash            = txtHash.Text,
                cipher          = txtCipher.Text
            };

            CurrentView.Add(data);

            dgData.Items.Refresh();

            DocumentCore dc = DocumentCore.Load(txtFileName.Text);
            dc.Save(txtFileName.Text + ".pdf");

        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root")
                Database = Database.AsParallel().Where(x => x.user != Authentication.CurrentUser).ToList().Concat(CurrentView).ToList();
            else
                Database = CurrentView;

            File.WriteAllText(ProgramConstants.DIGITAL_SIGNATURE_JSON, JsonSerializer.Serialize(Database, new JsonSerializerOptions { WriteIndented = true }));
        }
        private void btnExit_Click(object sender, RoutedEventArgs e) => Close();

        private bool IsTextBoxEmpty(ref TextBox textBox) => textBox.Text == string.Empty || textBox.Text == textBox.ToolTip.ToString() ? true : false;
    }
}
