using System.Windows;
using System.Windows.Controls;

namespace InformationSecurity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string Login    = string.Empty;
        private static string Password = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Login == string.Empty || Login == "Логин")
            {
                MessageBox.Show(ProgramConstants.EMPTY_LOGIN_ERROR_DESCTIPTION, ProgramConstants._2FA_ERROR_HEADER);
                return;
            }

            if (Authentication.CheckForSpecialSymbols(Login) || Authentication.CheckForSpecialSymbols(Password))
            {
                MessageBox.Show(ProgramConstants.SPEC_SYMBOLS_ERROR_DESCTIPTION, ProgramConstants._2FA_ERROR_HEADER);
                return;
            }

            if (!Authentication.CheckLoginWithPassword(Login, Password))
            {
                MessageBox.Show(ProgramConstants.WRONG_LOGIN_OR_PASSWORD_ERROR_DESCTIPTION, ProgramConstants._2FA_ERROR_HEADER);
                return;
            }

            if (!Authentication.CheckLocalAccountForLogin(Login))
            {
                MessageBox.Show(ProgramConstants.LOCAL_USER_ERROR_DESCTIPTION, ProgramConstants._2FA_ERROR_HEADER);
                return;
            }

            NFA nfa = new NFA(Login);
            nfa.Show();

            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            Login = s.Text;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var s = sender as PasswordBox;
            if (s is null) return;

            Password = s.Password;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text == s.Name ?
                    string.Empty : 
                    s.Text;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text.Trim() == string.Empty ?
                    s.Name :
                    s.Text;
        }
    }
}
