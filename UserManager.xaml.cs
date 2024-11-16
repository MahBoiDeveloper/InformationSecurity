using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Runtime;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var user in Authentication.Users)
            {
                MessageBox.Show(user.login + ", " + user.password + ", " + user.allowed_local_account + ", " + user.code);
            }
        }
    }
}
