using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static InformationSecurity.KuznechikManager;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для RSAManager.xaml
    /// </summary>
    public partial class RSAManager : Window
    {
        public class RSAData
        {
            public string user { get; set; }
            public string open_exponent { get; set; }
            public string closed_exponent { get; set; }
            public string multiplication { get; set; }
            public string message { get; set; }
            public string cipher { get; set; }
        }

        BigInteger openExp, closedExp, mult;

        public List<RSAData> Database { get; set; }
        public List<RSAData> CurrentView { get; set; }
        public RSAManager()
        {
            Database = JsonSerializer.Deserialize<List<RSAData>>(File.ReadAllText(ProgramConstants.RSA_JSON));

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

            File.WriteAllText(ProgramConstants.RSA_JSON, JsonSerializer.Serialize(Database, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void btnGetRandomKeys_Click(object sender, RoutedEventArgs e)
        {
            RSA rsa = new RSA();
            (openExp, mult)   = rsa.GetPublicKey();
            (closedExp, mult) = rsa.GetPrivateKey();

            txtOpenExponent.Text   = openExp.ToString();
            txtClosedExponent.Text = closedExp.ToString();
            txtMultiplication.Text = mult.ToString();
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

        private void btnReturn_Click(object sender, RoutedEventArgs e) => Close();

        private void btnSaveToTable_Click(object sender, RoutedEventArgs e)
        {
            if (IsTextBoxEmpty(ref txtInput) || IsTextBoxEmpty(ref txtOutput))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_SAVE_DESCRIPTION, ProgramConstants.RSA_ERROR_HEADER);
                return;
            }

            CurrentView.Add(new RSAData
            {
                user            = Authentication.CurrentUser,
                open_exponent   = openExp.ToString(),
                closed_exponent = closedExp.ToString(),
                multiplication  = mult.ToString(),
                message         = txtInput.Text,
                cipher          = txtOutput.Text
            });

            dgData.Items.Refresh();
        }

        private async void btnCipher_Click(object sender, RoutedEventArgs e)
        {
            if (!IsKeysComplete())
                return;

            if (IsTextBoxEmpty(ref txtInput))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_ENCRYPT_DESCRIPTION, ProgramConstants.RSA_ERROR_HEADER);
                return;
            }

            txtOutput.Text = SetUpRSA().Encrypt(txtInput.Text);

            UpdateCache();
        }

        private void btnDecipher_Click(object sender, RoutedEventArgs e)
        {
            if (!IsKeysComplete())
                return;

            if (IsTextBoxEmpty(ref txtOutput))
            {
                MessageBox.Show(ProgramConstants.NO_DATA_TO_DECRYPT_DESCRIPTION, ProgramConstants.RSA_ERROR_HEADER);
                return;
            }

            txtInput.Text = SetUpRSA().Decrypt(txtOutput.Text);

            UpdateCache();
        }
        
        private bool IsKeysComplete()
        {
            if (IsTextBoxEmpty(ref txtOpenExponent) || IsTextBoxEmpty(ref txtClosedExponent) || IsTextBoxEmpty(ref txtMultiplication))
            {
                MessageBox.Show(ProgramConstants.RSA_EMPTY_FIELDS, ProgramConstants.RSA_ERROR_HEADER);
                return false;
            }

            string msg = "Hello";
            RSA rsa = SetUpRSA();

            if (rsa.Decrypt(rsa.Encrypt(msg)) != msg)
            {
                MessageBox.Show(ProgramConstants.RSA_WRONG_KEYS_DESCRIPTION, ProgramConstants.RSA_ERROR_HEADER);
                return false;
            }

            return true;
        }

        private bool IsTextBoxEmpty(ref TextBox textBox) => textBox.Text == string.Empty || textBox.Text == textBox.ToolTip.ToString() ? true : false;

        private RSA SetUpRSA() => new RSA(BigInteger.Parse(txtOpenExponent.Text), BigInteger.Parse(txtClosedExponent.Text), BigInteger.Parse(txtMultiplication.Text));

        private void UpdateCache()
        {
            openExp = BigInteger.Parse(txtOpenExponent.Text);
            closedExp = BigInteger.Parse(txtClosedExponent.Text);
            mult = BigInteger.Parse(txtMultiplication.Text);
        }
    }
}
