using Ekzamen.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private readonly DbEntities _db = DbEntities.GetContext();
        private readonly User _user;
        public CartPage()
        {
            _user = _db.Users.FirstOrDefault();
            Order = new Order() { User = _user };
            InitializeComponent();

            this.DataContext = this;
        }

        public BindingList<Product> Products => _db.Products.Local.ToBindingList();
        public Order Order { get; set; }
        public ObservableCollection<Order_Product> Cart { get; set; } = new ObservableCollection<Order_Product>();

        private void AddProductToCart_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var product = btn.DataContext as Product;
            var order_product = Cart.Where(p => p.Product.Id == product.Id).FirstOrDefault();

            if (order_product != null)
            {
                if (product.Count == order_product.Count)
                {
                    MessageBox.Show("Добавить больше не получится", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                order_product.Count++;
                cartBox.Items.Refresh();
                txtCost.Text = string.Format("{0:c}", Cart.Sum(p => p.Count * p.Product.Price));
                return;
            }
            Cart.Add(new Order_Product()
            {
                Order = this.Order,
                Product = product,
                Count = 1
            });
            txtCost.Text = string.Format("{0:c}", Cart.Sum(p => p.Count * p.Product.Price));
        }

        private void AcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Count == 0) return;
            foreach(var order in Cart)
            {
                var product = Products.FirstOrDefault(p => p.Id == order.Product.Id);
                product.Count = product.Count - order.Count;
            }
            txtCost.Text = string.Format("{0:c}", 0);
            Order.Date= DateTime.Now;
            Order.Order_Product.AddRange(Cart);
            _db.Orders.Add(Order);
            _db.SaveChanges();
            Order = new Order() { User = _user};
            Cart = new ObservableCollection<Order_Product>();
            this.DataContext = null;
            this.DataContext = this;
        }

        private void ToPdf_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() != true) return;
            dialog.PrintVisual(mainframe, "Заказ");
        }

        private void DelProductToCart_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var product = btn.DataContext as Order_Product;
            if (product != null)
            {
                if (product.Count == 1)
                {
                    Cart.Remove(product);
                    if (Cart.Count == 0)
                    {
                        txtCost.Text = string.Format("{0:c}", 0);
                    }
                    else
                    {
                        txtCost.Text = string.Format("{0:c}", Cart.Sum(p => p.Count * p.Product.Price));
                    }
                    return;
                }
                product.Count--;
                cartBox.Items.Refresh();
                txtCost.Text = string.Format("{0:c}", Cart.Sum(p => p.Count * p.Product.Price));
                return;
            }
            txtCost.Text = string.Format("{0:c}", Cart.Sum(p => p.Count * p.Product.Price));
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var product = btn.DataContext as Product;
            this.NavigationService.Navigate(new AddEditProduct(product));
            _db.SaveChanges();
            productList.Items.Refresh();
        }

        private void DelProduct_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var product = btn.DataContext as Product;
            if (product.Order_Product.Count != 0)
            {
                MessageBox.Show("Нельзя удалить продукт, потому что он оформлен в заказе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var answer = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (answer != MessageBoxResult.Yes) return;
            _db.Products.Remove(product);
            _db.SaveChanges();
            productList.Items.Refresh();
            this.DataContext = null;
            this.DataContext = this;
            MessageBox.Show("Запись удалена!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddEditProduct(new Product()));
            //_db.SaveChanges();
            //_db.Products.Load();
            //productList.Items.Refresh();
            //this.DataContext = null;
            //this.DataContext = this;
        }
    }
}
