using System.Text;
using System.Windows;
using System.Security;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                MessageBox.Show("Авторизация под пустым логином невозможна!", ProgramConstants._2FA_ERROR_HEADER);
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

            if (s.Name == "Логин")
                Login = s.Text;
            else
                Password = s.Text;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text = 
                (s.Text == "Логин") ||
                (s.Text == "Пароль") ||
                (s.Text == "Повторите пароль") ?
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
