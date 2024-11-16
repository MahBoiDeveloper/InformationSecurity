using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для RSAManager.xaml
    /// </summary>
    public partial class RSAManager : Window
    {
        public class RSAData
        {
            public string user { get; set; }
            public string open_exponent { get; set; }
            public string closed_exponent { get; set; }
            public string multiplication { get; set; }
            public string message { get; set; }
            public string cipher { get; set; }
        }
        public List<RSAData> Database { get; set; }
        public List<RSAData> CurrentView { get; set; }
        public RSAManager()
        {
            Database = JsonSerializer.Deserialize<List<RSAData>>(File.ReadAllText(ProgramConstants.RSA_JSON));
            CurrentView = Authentication.CurrentUser != "admin" && Authentication.CurrentUser != "root" ?
                          Database.AsParallel().Where(x => x.user == Authentication.CurrentUser).ToList() :
                          Database;

            InitializeComponent();
        }
    }
}
