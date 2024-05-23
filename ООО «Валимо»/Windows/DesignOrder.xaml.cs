using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ООО__Валимо_.Classes;
using ООО__Валимо_.Entity;

namespace ООО__Валимо_.Windows
{
    /// <summary>
    /// Логика взаимодействия для DesignOrder.xaml
    /// </summary>
    public partial class DesignOrder : Window
    {
        List<ProductInOrder> listOrder;
        public DesignOrder(List<ProductInOrder> listOrder)
        {
            InitializeComponent();
            this.listOrder = listOrder;	//Перенести из параметра в глобальный элемент окна
            ShowInfo();

            cbClient.DisplayMemberPath = "fullName";
            cbClient.ItemsSource = Helper.DB.Client.ToList();
            cbClient.SelectedIndex = 0;
        }

        private void ShowInfo()
        {
            decimal total = 0;
            listBoxProductsInOrder.ItemsSource = null;
            for (int i = 0; i < listOrder.Count; i++)
            {
                if (listOrder[i].extendProduct.image == null || listOrder[i].extendProduct.image == "")
                    listOrder[i].extendProduct.imagePath = "../../Images/picture.png";
                else
                    listOrder[i].extendProduct.imagePath = "../../Images/" + listOrder[i].extendProduct.image;
                total += listOrder[i].extendProduct.costWithDis * listOrder[i].count;
            }
            //Перенести данные о заказе в интерфейс
            listBoxProductsInOrder.ItemsSource = listOrder;
            tbTotal.Text = $"Итого: {total} руб.";
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
                if (listOrder.Count <= 0)           //Пустой заказ
                {
                    MessageBox.Show("Корзина пуста!");
                    return;
                }
                //Формируем объект для таблицы Order БД
                Order orderDB = new Order();
                //Заполняем все обязательные поля кроме авто инкрементируемых
                orderDB.dateRegistration = DateTime.Now;           //Дата заказа
                orderDB.idClient = (cbClient.SelectedItem as Client).id;
                orderDB.idManager = Helper.User.id;
                orderDB.status = Helper.DB.Status.ToList().Find(s => s.id == 1).id;
                
            //Статус - новый
                                                            //Добавляем заказ в виртуальную таблицу
                Helper.DB.Order.Add(orderDB);
                try
                {
                    //Обновление БД
                    Helper.DB.SaveChanges();
                    //Все товары в этом заказе сохраняем в таблице OrderProduct
                    foreach (var productinorder in listOrder)
                    {
                        //Создаем объект для сохранения
                        ProductInOrder orderProduct = new ProductInOrder();
                        //Берем Id только что созданного заказа, чтобы записать его для каждого товара
                        orderProduct.idOrder = orderDB.id;
                        //Артикль
                        orderProduct.idProduct = productinorder.extendProduct.id;
                        //Количество
                        orderProduct.count = productinorder.count;
                        //Добавляем заполненный товар заказа сначала в виртуальную таблицу
                        Helper.DB.ProductInOrder.Add(orderProduct);
                    }
                    Helper.DB.SaveChanges();            //Обновляем БД
                    listOrder.Clear();                  //Очищаем корзину
                }
                catch
                {
                    MessageBox.Show("При добавлении данных возникла ошибка");
                    return;
                }
            Close();
        }


        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            change(sender, "remove");
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            change(sender, "+");
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            change(sender, "-");
        }

        private void change(object sender, string act)
        {
            ProductInOrder pio = (sender as Button).DataContext as ProductInOrder;
            //MessageBox.Show(pio.id.ToString());
            int id = listOrder.IndexOf(pio);

            switch (act)
            {
                case "+":
                    listOrder[id].count++;
                    break;
                case "-":
                    listOrder[id].count--;
                    if (listOrder[id].count == 0) change(sender, "remove");
                    break;
                case "remove":
                    listOrder.Remove(listOrder[id]);
                    break;
            }
            ShowInfo();
        }
    }
}
