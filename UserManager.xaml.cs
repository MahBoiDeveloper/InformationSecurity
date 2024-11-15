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
#pragma warning disable CS8618
    public class User
    {
        public string Номер { get; set; }
        public string Логин { get; set; }
        public string Пароль { get; set; }
        public string Локалка { get; set; }
        public string КодыДоступа { get; set; }
        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Номер;
                    case 1:
                        return Логин;
                    case 2:
                        return Пароль;
                    case 3:
                        return Локалка;
                    case 4:
                        return КодыДоступа;
                    default:
                        return "";
                }
            }
            set
            {
                this[index] = value;
            }
        }

    }
#pragma warning restore CS8618

    /// <summary>
    /// Логика взаимодействия для UserManager.xaml
    /// </summary>
    public partial class UserManager : Window
    {
        public ObservableCollection<User> dbUsers { get; set; } = new ObservableCollection<User>();
        public UserManager()
        {
            int i = 0;
            JsonElement.ArrayEnumerator userdata = JsonDocument.
                                               Parse(File.ReadAllText(ProgramConstants.USERS_JSON)).
                                               RootElement.
                                               GetProperty("userdata").
                                               EnumerateArray();

            foreach (var user in userdata)
            {
                dbUsers.Add(new User()
                {
                    Номер = (++i).ToString(),
                    Логин = user.GetProperty("login").ToString(),
                    Пароль = user.GetProperty("password").ToString(),
                    Локалка = user.GetProperty("allowed_local_account").ToString(),
                    КодыДоступа = user.GetProperty("codes").ToString()
                });

            }

            InitializeComponent();
            dgUsers.ItemsSource = dbUsers;
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            if (s is null) return;

            Registration rg = new Registration();
            rg.ShowDialog();
        }

        private void dgUsers_CurrentCellChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Меняем данные");
            MessageBox.Show("((User)dgUsers.CurrentCell.Item).Номер = " + (Convert.ToInt32(((User)dgUsers.CurrentCell.Item).Номер) - 1).ToString());
            dbUsers[Convert.ToInt32(((User)dgUsers.CurrentCell.Item).Номер) - 1] = (User)dgUsers.CurrentCell.Item;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dynamic json = JsonSerializer.Deserialize<object>(File.ReadAllText(ProgramConstants.USERS_JSON));

            foreach (var user in dbUsers)
            {
                json["userdata"][user.Номер]["login"] = user.Логин;
                json["userdata"][user.Номер]["password"] = user.Пароль;
                json["userdata"][user.Номер]["allowed_local_account"] = user.Локалка;
                json["userdata"][user.Номер]["codes"] = user.КодыДоступа;
            }
            foreach (var user in dbUsers)
            {
                MessageBox.Show(JsonSerializer.Serialize<User>(user));
            }
        }
    }
}
