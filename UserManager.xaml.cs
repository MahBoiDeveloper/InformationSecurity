using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text.Json;
using System.Xml;

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
            dgUsers.ItemsSource = Authentication.Users;
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            if (s is null) return;

            Registration rg = new Registration();
            rg.ShowDialog();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(ProgramConstants.USERS_JSON, JsonSerializer.Serialize(Authentication.Users, new JsonSerializerOptions { WriteIndented = true} ));
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
