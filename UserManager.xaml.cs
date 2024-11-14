using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text.Json;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для UserManager.xaml
    /// </summary>
    public partial class UserManager : Window
    {
        JsonElement.ArrayEnumerator userdata = JsonDocument.
                                               Parse(File.ReadAllText(ProgramConstants.USERS_JSON)).
                                               RootElement.
                                               GetProperty("userdata").
                                               EnumerateArray();
        public UserManager() => InitializeComponent();


        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            if (s is null) return;

            Registration rg = new Registration();
            rg.ShowDialog();
        }

        private void TextBlock_Initialized(object sender, EventArgs e)
        {
            foreach (var user in userdata)
            {

            }
        }
    }
}
