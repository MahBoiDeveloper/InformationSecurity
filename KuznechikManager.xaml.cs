using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            File.WriteAllText(ProgramConstants.KUZNECHIK_JSON, JsonSerializer.Serialize(Authentication.Users, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

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
            byte[] key1;
            byte[] key2;

            if (IsTextBoxEmpty(ref txtKey1))
                key1 = Kuznechik.FirstKey;
            else
                key1 = Encoding.Default.GetBytes(txtKey1.Text);

            if (IsTextBoxEmpty(ref txtKey2))
                key2 = Kuznechik.FirstKey;
            else
                key2 = Encoding.Default.GetBytes(txtKey2.Text);

            return new Kuznechik(key1, key2);
        }
    }
}
