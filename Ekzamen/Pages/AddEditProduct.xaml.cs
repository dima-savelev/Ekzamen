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

namespace Ekzamen.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Page
    {
        public readonly DbEntities _db = DbEntities.GetContext();
        public readonly Product _product;
        public AddEditProduct(Product product)
        {
            InitializeComponent();
            _product = product;
            txtName.Text = _product.Name;
            txtDescription.Text = _product.Description;
            txtPrice.Text = _product.Price.ToString();
            txtCount.Text = _product.Count.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errors.AppendLine("Введите имя");
            }
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                errors.AppendLine("Введите описание");
            }
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                errors.AppendLine("Цена введена неверно");
            }
            if (!int.TryParse(txtCount.Text, out int count) || count < 0)
            {
                errors.AppendLine("Кол-во введено неверно");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _product.Name = txtName.Text;
            _product.Description = txtDescription.Text;
            _product.Price= price;
            _product.Count = count;
            if (_db.Products.Find(_product.Id) == null)
            {
                _db.Products.Add(_product);
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Успешно сохранено!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }
    }
}
