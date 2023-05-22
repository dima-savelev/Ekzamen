using Ekzamen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ekzamen
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToCartPage_Click(object sender, RoutedEventArgs e)
        {
            pageContainer.Navigate(new Pages.CartPage());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Autorization autoriz = new Autorization();
            autoriz.Owner = MainWindow1;
            autoriz.ShowDialog();
            if (DataLogin.Login == false)
            {
                Close();
            }
            string right;
            if (DataLogin.Rule == "A")
            {
                right = "Администратор";
            }
            else
            {
                right = "Пользователь";
            }
        }
    }
}
