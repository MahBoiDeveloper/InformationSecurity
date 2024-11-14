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
        public Manager(string lgn)
        {
            Login = lgn;

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

            Registration mainWindow = new Registration();
            mainWindow.Show();
            Close();
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
    }
}
