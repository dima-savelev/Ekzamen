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
using System.Windows.Shapes;

namespace Ekzamen.Data
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        private readonly DbEntities db = DbEntities.GetContext();
        public Autorization()
        {
            InitializeComponent();
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            txtLogin.Focus();
            DataLogin.Login = false;
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == string.Empty || txtPas.Password == string.Empty)
            {
                MessageBox.Show("Логин или пароль были не введены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var user = db.Users.Where(p => p.Name.ToLower() == txtLogin.Text.ToLower()).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtLogin.Focus();
                return;
            }
            if (user.Password != txtPas.Password)
            {
                MessageBox.Show("Пароль неверен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DataLogin.Login = true;
            DataLogin.Name = user.Name;
            DataLogin.Rule= user.Rule;
            Close();
        }

        private void Esc(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
