using System.Windows;
using System.Windows.Controls;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        public string Login;
        public Manager()
        {
            Login = Authentication.CurrentUser;
            InitializeComponent();
        }

        public void Button_ClickLogOut(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            if (s == null) return;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        public void Button_ClickUserManagement(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            if (s == null) return;

            UserManager um = new UserManager();
            um.ShowDialog();
        }

        private void Button_InitializedUserManagement(object sender, EventArgs e)
        {
            var s = sender as Button;
            if (s == null) return;

            if (!(Login == "admin" || Login == "root"))
            {
                s.IsEnabled = false;
                s.Visibility = Visibility.Hidden;
                return;
            }
        }

        private void btnKuz_Click(object sender, RoutedEventArgs e)
        {
            KuznechikManager km = new KuznechikManager();
            km.ShowDialog();
        }

        private void btnRSA_Click(object sender, RoutedEventArgs e)
        {
            RSAManager rm = new RSAManager();
            rm.ShowDialog();
        }

        private void DS_Click(object sender, RoutedEventArgs e)
        {
            DigitalSignatureManager dsm = new DigitalSignatureManager();
            dsm.ShowDialog();
        }
    }
}
