using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для NFA.xaml
    /// </summary>
    public partial class NFA : Window
    {
        private string Code = string.Empty;
        private string Login = string.Empty;
        public NFA(string login)
        {
            Login = login;
            InitializeComponent();
        }

        private void Button_Click(object s, RoutedEventArgs e)
        {
            if (Login == string.Empty || Login == "Логин")
            {
                MessageBox.Show("Авторизация под пустым логином невозможна!", ProgramConstants._2FA_ERROR_HEADER);
                return;
            }

            if (!Authentication.CheckCodeForLogin(Login, Code))
            {
                MessageBox.Show(ProgramConstants.CODE_TO_LOGIN_ERROR_DESCTIPTION, ProgramConstants._2FA_ERROR_HEADER);
                return;
            }

            Manager mng = new Manager();
            mng.Show();

            Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                (s.Text == "Код доступа") ?
                    string.Empty :
                    s.Text;
        }

        private void TextBox_LostFocus (object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text.Trim() == string.Empty ?
                    "Код доступа" :
                    s.Text;
        }

        private void TextBox_TextChanged (object sender, TextChangedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            Code = s.Text;
        }

    }
}
