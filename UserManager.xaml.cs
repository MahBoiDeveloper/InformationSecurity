using System.Windows;
using System.Windows.Controls;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для UserManager.xaml
    /// </summary>
    public partial class UserManager : Window
    {
        public UserManager()
        {
            InitializeComponent();
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            if (s is null) return;

            Registration rg = new Registration();
            rg.ShowDialog();
        }
    }
}
