using ООО__Валимо_.Entity;
using ООО__Валимо_.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using System.Data.SqlClient;
using ООО__Валимо_.Windows;

namespace ООО__Валимо_
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {

        int cntProducts, cntAllProducts;
        List<Product> products;
        List<ProductInOrder> inOrder = new List<ProductInOrder>();
        List<string> genriesStr;
        List<Genre> genries;
        public Catalog()
        {
            InitializeComponent();
            updateListBox();
            genries = new List<Genre>();
            genriesStr = new List<string>();
            genriesStr.Add("По умолчанию");
            foreach (Genre genre in Helper.DB.Genre)
            {
                genriesStr.Add(genre.name);
                genries.Add(genre);
            }
            comboBoxGenre.ItemsSource = null;
            comboBoxGenre.ItemsSource = genriesStr;
            comboBoxGenre.SelectedIndex = 0;
            cntAllProducts = Helper.DB.Product.Count();
            tbResult.Text = $"Найдено {cntAllProducts} из {cntAllProducts}";

            comboBoxPrices.SelectedIndex = 0;
            comboBoxGenre.SelectedIndex = 0;
            comboBoxSort.SelectedIndex = 0;

            listBoxProducts.ContextMenu = null;
            if (Helper.User != null)
            {
                if (Helper.User.Role1.name == "Администратор")
                {
                    listBoxProducts.ContextMenu = (ContextMenu)FindResource("ItemContextMenuAdmin");
                }
                if (Helper.User.Role1.name == "Менеджер")
                {
                    listBoxProducts.ContextMenu = (ContextMenu)FindResource("ItemContextMenuClient");
                }
            }
            btnDesign.Visibility = Visibility.Hidden;
        }

        public void updateListBox()
        {
            products = new List<Product>();
            foreach (Product product in Helper.DB.Product)
            {
                if (product.image == null || product.image == "")
                    product.imagePath = "../../Images/picture.png";
                else
                    product.imagePath = "../../Images/" + product.image;
                
                product.costWithDis = product.cost * Convert.ToDecimal(100.0-product.discount)/100;

                products.Add(product);
            }
            listBoxProducts.ItemsSource = products;
            cntAllProducts = products.Count;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void showProducts()
        {
            updateListBox();

            if (comboBoxGenre == null || comboBoxPrices == null || comboBoxPrices == null || tbSearch == null) return;

            switch (comboBoxSort.SelectedIndex)
            {
                case 1:
                    products = products.OrderBy(x => x.cost).ToList();
                    break;
                case 2:
                    products = products.OrderByDescending(x => x.cost).ToList();
                    break;
            }

            if (comboBoxGenre.SelectedIndex > 0) 
                products = products.Where(p => p.Genre1.id == comboBoxGenre.SelectedIndex).ToList();

            switch (comboBoxPrices.SelectedIndex)
            {
                case 1:
                    products = products.Where(p => p.discount < 0.1).ToList();
                    break;
                case 2:
                    products = products.Where(p => p.discount < 0.15 && p.discount >= 0.1).ToList();
                    break;
                case 3:
                    products = products.Where(p => p.discount >= 0.15).ToList();
                    break;
            }
            string text = tbSearch.Text;
            if (text != "")
            {
                products = products.Where(p => p.name.ToLower().Contains(text)).ToList();
            }
            listBoxProducts.ItemsSource = products;
            cntProducts = products.Count;
            tbResult.Text = $"Найдено {cntProducts} из {cntAllProducts}";
        }

        private void MenuItemClient_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxProducts.SelectedIndex < 0) return;
            Product selProduct = listBoxProducts.SelectedItem as Product;
            btnDesign.Visibility = Visibility.Visible;

            if (inOrder.Find(pio => pio.idProduct == selProduct.id) != null)
            {
                (inOrder.Find(pio => pio.idProduct == selProduct.id)).count++;
            }
            else
            {
                ProductInOrder pio = new ProductInOrder();
                pio.idProduct = selProduct.id;
                pio.extendProduct = selProduct;
                pio.count = 1;
                inOrder.Add(pio);
            }

        }



        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            showProducts();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showProducts();
        }

        private void MenuItemAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxProducts.SelectedIndex < 0) return;
            Product delProduct = listBoxProducts.SelectedItem as Product;
            switch ((sender as MenuItem).Name)
            {
                case "del":
                    deleteProduct(delProduct);
                    break;
                case "edit":
                    WorkWithProduct workWithProductEdit = new WorkWithProduct("edit", delProduct);
                    Hide();
                    workWithProductEdit.ShowDialog();
                    Show();
                    break;
                case "add":
                    WorkWithProduct workWithProductAdd = new WorkWithProduct("add", null);
                    Hide();
                    workWithProductAdd.ShowDialog();
                    Show();
                    break;
            }
            showProducts();
        }

        private void btnDesign_Click(object sender, RoutedEventArgs e)
        {
            DesignOrder designOrder = new DesignOrder(inOrder);
            designOrder.ShowDialog();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            showProducts();
        }

        private void deleteProduct(Product delProduct)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот товар?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Проверяем результат нажатия кнопки в диалоговом окне
            if (result == MessageBoxResult.No) return;
            
            delProduct = listBoxProducts.SelectedItem as Product;
            if (Helper.DB.ProductInOrder.ToList().Find(pio => pio.idProduct == delProduct.id) != null) {
                MessageBox.Show("Удаление невозможно! Этот товар состоит в заказе!");
                return;
            }

            Helper.DB.Product.Remove(delProduct);
            Helper.DB.SaveChanges();
        }
    }
}
