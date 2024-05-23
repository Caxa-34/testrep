using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using ООО__Валимо_.Classes;
using ООО__Валимо_.Entity;

namespace ООО__Валимо_.Windows
{
    /// <summary>
    /// Логика взаимодействия для WorkWithProduct.xaml
    /// </summary>
    public partial class WorkWithProduct : Window
    {
        string type;
        List<Genre> genres;
        Product product;
        public WorkWithProduct(string type, Product product)
        {
            InitializeComponent();
            genres = Helper.DB.Genre.ToList();
            cbGenre.DisplayMemberPath = "name";
            cbGenre.SelectedValuePath = "id";
            cbGenre.ItemsSource = genres;
            cbGenre.SelectedIndex = 0;
            this.type = type;
            if (type == "add") 
            {
                tbTitle.Text = "Добавление товара";
                btnApply.Content = "Добавить";
                this.product = new Product();
            }
            if (type == "edit")
            {
                tbTitle.Text = "Редактирование товара";
                btnApply.Content = "Перезаписать";


                this.product = product;
                tbName.Text = product.name;
                cbGenre.SelectedIndex = product.genre - 1;
                tbCost.Text = product.cost.ToString();
                tbDiscount.Text = product.discount.ToString();
                tbCreator.Text = product.creator.ToString();
                if (product.image != null) tbImage.Text = product.image.ToString();
                if (product.description != null) tbDescription.Text = product.description.ToString();


            }
        }

        private bool checkValues()
        {
            try
            {
                string name = tbName.Text;
                int genre = cbGenre.SelectedIndex;
                int cost = Convert.ToInt32(tbCost.Text);
                float discount = Convert.ToSingle(tbDiscount.Text);
                string creator = tbCreator.Text;
                string image = tbImage.Text;
                string description = tbDescription.Text;

                if (discount >100 || discount < 0) return false;
                if (cost <= 0) return false;
                if (name == "" || creator == "") return false;
            }
            catch {
                //MessageBox.Show("Ошибка конвертации");
                return false;
            }
            return true;
        }

        private void apply()
        {
            //MessageBox.Show(type);
            if (!checkValues()) return;
            //MessageBox.Show("Данные валидны");
            string server = "CAXALT\\SQLEXPRESS";
            string database = "DBValimo";
            string connectionString = $"Data Source={server};Initial Catalog={database};Integrated Security=True";
            string sqlQuery = "";

            string name = tbName.Text;
            int genre = cbGenre.SelectedIndex + 1;
            int cost = Convert.ToInt32(tbCost.Text);
            float discount = Convert.ToSingle(tbDiscount.Text);
            string creator = tbCreator.Text;
            string image = tbImage.Text;
            string description = tbDescription.Text;

            if (type == "add")
            {
                sqlQuery = $"INSERT INTO Product (name, genre, cost, discount, creator, image, description) VALUES ('{name}', {genre}, {cost}, {discount}, '{creator}', '{image}', '{description}')";
            }
            if (type == "edit")
            {
                int id = product.id;
                sqlQuery = $"UPDATE Product SET name = '{name}', genre = {genre}, cost = {cost}, discount = {discount}, creator = '{creator}', image = '{image}', description = '{description}' WHERE id = {id}";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected <= 0) MessageBox.Show("Не удалось выполнить изменения.");
                    else if (type == "edit") MessageBox.Show("Для отображения изменений перезагрузите приложение.");
                }
            }
            Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            apply();
        }
    }
}
