using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InformationSecurity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration rg = new Registration();
            rg.Show();
            Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text = 
                (s.Text == "Логин") ||
                (s.Text == "Пароль") ||
                (s.Text == "Повторите пароль") ?
                    string.Empty : 
                    s.Text;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var s = sender as TextBox;
            if (s is null) return;

            s.Text =
                s.Text.Trim() == string.Empty ?
                    s.Name :
                    s.Text;
        }
    }
}
