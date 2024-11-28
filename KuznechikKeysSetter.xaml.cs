using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    /// Логика взаимодействия для KuznechikKeysSetter.xaml
    /// </summary>
    public partial class KuznechikKeysSetter : Window
    {
        public KuznechikKeysSetter()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string[] arr = new string[] 
            {
                txt1.Text, 
                txt2.Text, 
                txt3.Text, 
                txt4.Text, 
                txt5.Text, 
                txt6.Text, 
                txt7.Text, 
                txt8.Text, 
                txt9.Text,
                txt10.Text
            };
            
            try
            {
                Kuznechik kuzya = new Kuznechik(arr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("При тестовой попытке запуска Кузнечика возникла ошибка: " + ex.Message, "Ошибка в ключах");
                return;
            }

            KuznechikManager.Keys = arr;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            byte[] buff = new byte[16];

            rng.NextBytes(buff);
            txt1.Text = Convert.ToHexString(buff);

            rng.NextBytes(buff);
            txt2.Text = Convert.ToHexString(buff);

            Kuznechik kz = new Kuznechik(Convert.FromHexString(txt1.Text), Convert.FromHexString(txt2.Text));

            txt3.Text = Convert.ToHexString(kz.KEY[2]);
            txt4.Text = Convert.ToHexString(kz.KEY[3]);
            txt5.Text = Convert.ToHexString(kz.KEY[4]);
            txt6.Text = Convert.ToHexString(kz.KEY[5]);
            txt7.Text = Convert.ToHexString(kz.KEY[6]);
            txt8.Text = Convert.ToHexString(kz.KEY[7]);
            txt9.Text = Convert.ToHexString(kz.KEY[8]);
            txt10.Text = Convert.ToHexString(kz.KEY[9]);
        }
    }
}
