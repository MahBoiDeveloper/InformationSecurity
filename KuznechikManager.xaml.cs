using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Логика взаимодействия для KuznechikManager.xaml
    /// </summary>
    public partial class KuznechikManager : Window
    {
        public class KuznechikData
        {
            public string user {get; set;} = string.Empty;
            public string first_key {get; set;} = string.Empty;
            public string second_key {get; set;} = string.Empty;
            public string message {get; set;} = string.Empty;
            public string cipher { get; set; } = string.Empty;
        }
        public List<KuznechikData>? Database { get; set; }
        public List<KuznechikData>? CurrentView { get; set; }
        public KuznechikManager()
        {
            Database = JsonSerializer.Deserialize<List<KuznechikData>>(File.ReadAllText(ProgramConstants.KUZNECHIK_JSON));
            
            if (Database == null) return;

            CurrentView = Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root" ?
                          Database.AsParallel().Where(x => x.user == Authentication.CurrentUser).ToList() :
                          Database;

            InitializeComponent();

            dgData.ItemsSource = CurrentView;

            if (Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root")
                clmLogin.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(ProgramConstants.KUZNECHIK_JSON, JsonSerializer.Serialize(Authentication.Users, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text == s.ToolTip.ToString() ?
                    string.Empty :
                    s.Text;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text.Trim() == string.Empty ?
                    s.ToolTip.ToString() :
                    s.Text;
        }
    }
}
