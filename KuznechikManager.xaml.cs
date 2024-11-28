using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для KuznechikManager.xaml
    /// </summary>
    public partial class KuznechikManager : Window
    {
        public class KuznechikData
        {
            public string user { get; set; } = string.Empty;
            public string first_key { get; set; } = string.Empty;
            public string second_key { get; set; } = string.Empty;
            public string message { get; set; } = string.Empty;
            public string cipher { get; set; } = string.Empty;
        }
        public List<KuznechikData>? Database { get; set; }
        public List<KuznechikData>? CurrentView { get; set; }

        private byte[] key1;
        private byte[] key2;

        public static string[] Keys;

        public KuznechikManager()
        {
            Database = JsonSerializer.Deserialize<List<KuznechikData>>(File.ReadAllText(ProgramConstants.KUZNECHIK_JSON));
            
            if (Database == null) return;

            CurrentView = Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root" ?
                          Database.AsParallel().Where(x => x.user == Authentication.CurrentUser).ToList() :
                          Database;

            InitializeComponent();

            dgData.ItemsSource = CurrentView;

            if (Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root")
                clmLogin.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root")
                Database = Database.AsParallel().Where(x => x.user != Authentication.CurrentUser).ToList().Concat(CurrentView).ToList();
            else
                Database = CurrentView;

            File.WriteAllText(ProgramConstants.KUZNECHIK_JSON, JsonSerializer.Serialize(Database, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e) => Close();

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text == s.ToolTip.ToString() ?
                    string.Empty :
                    s.Text;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text.Trim() == string.Empty ?
                    s.ToolTip.ToString() :
                    s.Text;
        }

        private void btnCipher_Click(object sender, RoutedEventArgs e)
        {
            if (!IsKeysComplete())
                return;
            
            if (IsTextBoxEmpty(ref txtInput))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_ENCRYPT_DESCRIPTION, ProgramConstants.KUZNECHIK_ERROR_HEADER);
                return;
            }
            
            txtOutput.Text = SetUpKuznechik().Encrypt(txtInput.Text);
        }

        private void btnDecipher_Click(object sender, RoutedEventArgs e)
        {
            if (!IsKeysComplete())
                return;

            if (IsTextBoxEmpty(ref txtOutput))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_DECRYPT_DESCRIPTION, ProgramConstants.KUZNECHIK_ERROR_HEADER);
                return;
            }

            try
            {
                txtInput.Text = SetUpKuznechik().Decrypt(txtOutput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ProgramConstants.OUTPUT_DATA_IS_NOT_A_HEX_DESC + ex.Message, ProgramConstants.KUZNECHIK_ERROR_HEADER);
            }
        }

        private void btnSaveToTable_Click(object sender, RoutedEventArgs e)
        {
            if (IsTextBoxEmpty(ref txtInput) || IsTextBoxEmpty(ref txtOutput))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_SAVE_DESCRIPTION, ProgramConstants.KUZNECHIK_ERROR_HEADER);
                return;
            }

            CurrentView.Add(new KuznechikData {
                user = Authentication.CurrentUser,
                first_key = Keys == null? Convert.ToHexString(key1) : Keys[0],
                second_key = Keys == null ? Convert.ToHexString(key2) : Keys[1],
                message = txtInput.Text,
                cipher = txtOutput.Text
            });

            dgData.Items.Refresh();
        }

        private bool IsTextBoxEmpty(ref TextBox textBox) => textBox.Text == string.Empty || textBox.Text == textBox.ToolTip.ToString() ? true : false;
        
        private bool IsKeysComplete()
        {
            if (Encoding.Default.GetBytes(txtKey1.Text).Length != 16 && !IsTextBoxEmpty(ref txtKey1))
            {
                MessageBox.Show(string.Format(ProgramConstants.KEY1_INCOMPLETE_DESCRIPTION, Encoding.Default.GetBytes(txtKey1.Text).Length), ProgramConstants.KUZNECHIK_ERROR_HEADER);
                return false;
            }

            if (Encoding.Default.GetBytes(txtKey2.Text).Length != 16 && !IsTextBoxEmpty(ref txtKey2))
            { 
                MessageBox.Show(string.Format(ProgramConstants.KEY2_INCOMPLETE_DESCRIPTION, Encoding.Default.GetBytes(txtKey2.Text).Length), ProgramConstants.KUZNECHIK_ERROR_HEADER);
                return false;
            }

            return true;
        }

        private Kuznechik SetUpKuznechik()
        {

            if (IsTextBoxEmpty(ref txtKey1))
                key1 = Kuznechik.FirstKey;
            else
                key1 = Encoding.Default.GetBytes(txtKey1.Text);

            if (IsTextBoxEmpty(ref txtKey2))
                key2 = Kuznechik.SecondKey;
            else
                key2 = Encoding.Default.GetBytes(txtKey2.Text);

            if (Keys == null)
                return new Kuznechik(key1, key2);
            else
                return new Kuznechik(Keys);
        }

        private void btnSetAllKeys_Click(object sender, RoutedEventArgs e)
        {
            KuznechikKeysSetter kuznechikKeysSetter = new KuznechikKeysSetter();
            kuznechikKeysSetter.ShowDialog();
        }
    }
}
