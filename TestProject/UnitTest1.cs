using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ValimoUnitTest
{
    [TestClass]
    public class DiverseTests
    {
        /*
        [TestMethod]
        public void productDescriptionLengthTest()
        {
            // Arrange
            var product = new Product()
            {
                idProduct = 1,
                titleProduct = "Test Product",
                idCategory = 101,
                price = 100,
                discount = 20,
                maker = "Test Maker",
                image = "test.jpg",
                discription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio."
            };

            // Act
            int expectedLength = 50; // Ограничение описания продукта до 50 символов
            int actualLength = product.discription.Length;

            // Assert
            Assert.AreEqual(expectedLength, actualLength);
        }

        [TestMethod]
        public void Test_ProductSortingByPrice()
        {
            // Arrange
            var products = new List<Product>()
            {
                new Product() { idProduct = 1, titleProduct = "Product 1", price = 50 },
                new Product() { idProduct = 2, titleProduct = "Product 2", price = 30 },
                new Product() { idProduct = 3, titleProduct = "Product 3", price = 70 }
            };

            // Act
            var sortedProducts = products.OrderBy(p => p.price).ToList();

            // Assert
            CollectionAssert.AreEqual(products, sortedProducts);
        }

        [TestMethod]
        public void Test_ProductCostWithDiscountCalculation()
        {

            // Arrange
            var product = new Product()
            {
                idProduct = 1,
                titleProduct = "Test Product",
                idCategory = 101,
                price = 100,
                discount = 20, // 20% discount
                maker = "Test Maker",
                image = "test.jpg",
                discription = "Test Description"
            };

            // Act
            decimal expectedCostWithDiscount = 80; // 20% discount applied on 100
            decimal actualCostWithDiscount = product.price * (1 - (decimal)product.discount / 100);

            // Assert
            Assert.AreEqual(expectedCostWithDiscount, actualCostWithDiscount);
        }

        [TestMethod]
        public void Test_ProductOrdersCollection()
        {
            // Arrange
            var product = new Product();
            var order = new Order() { idOrder = 1 };

            // Act
            product.Order.Add(order);

            // Assert
            CollectionAssert.Contains((List<Order>)product.Order, order);
        }

        [TestMethod]
        public void Test_OrderInitialization()
        {
            // Arrange
            var order = new Order();

            // Act
            order.idOrder = 1;
            order.idProduct = 101;
            order.idClient = 201;
            order.idManager = 301;
            order.date = DateTime.Now;
            order.idStatus = 401;

            // Assert
            Assert.AreEqual(1, order.idOrder);
            Assert.AreEqual(101, order.idProduct);
            Assert.AreEqual(201, order.idClient);
            Assert.AreEqual(301, order.idManager);
            Assert.IsNotNull(order.date);
            Assert.AreEqual(401, order.idStatus);
        }

        */

                [TestMethod]
        public void productDescriptionLengthTest()
        {
            // Arrange
            var product = new Product()
            {
                idProduct = 1,
                titleProduct = "Test Product",
                idCategory = 101,
                price = 100,
                discount = 20,
                maker = "Test Maker",
                image = "test.jpg",
                discription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio."
            };

            // Act
            int expectedLength = 50; // Ограничение описания продукта до 50 символов
            int actualLength = product.discription.Length;

            // Assert
            Assert.AreEqual(expectedLength, actualLength);
        }

        [TestMethod]
        public void Test_ProductSortingByPrice()
        {
            // Arrange
            var products = new List<Product>()
            {
                new Product() { idProduct = 1, titleProduct = "Product 1", price = 50 },
                new Product() { idProduct = 2, titleProduct = "Product 2", price = 30 },
                new Product() { idProduct = 3, titleProduct = "Product 3", price = 70 }
            };

            // Act
            var sortedProducts = products.OrderBy(p => p.price).ToList();

            // Assert
            CollectionAssert.AreEqual(products, sortedProducts);
        }

        [TestMethod]
        public void Test_ProductCostWithDiscountCalculation()
        {

            // Arrange
            var product = new Product()
            {
                idProduct = 1,
                titleProduct = "Test Product",
                idCategory = 101,
                price = 100,
                discount = 20, // 20% discount
                maker = "Test Maker",
                image = "test.jpg",
                discription = "Test Description"
            };

            // Act
            decimal expectedCostWithDiscount = 80; // 20% discount applied on 100
            decimal actualCostWithDiscount = product.price * (1 - (decimal)product.discount / 100);

            // Assert
            Assert.AreEqual(expectedCostWithDiscount, actualCostWithDiscount);
        }

        [TestMethod]
        public void Test_ProductOrdersCollection()
        {
            // Arrange
            var product = new Product();
            var order = new Order() { idOrder = 1 };

            // Act
            product.Order.Add(order);

            // Assert
            CollectionAssert.Contains((List<Order>)product.Order, order);
        }

        [TestMethod]
        public void Test_OrderInitialization()
        {
            // Arrange
            var order = new Order();

            // Act
            order.idOrder = 1;
            order.idProduct = 101;
            order.idClient = 201;
            order.idManager = 301;
            order.date = DateTime.Now;
            order.idStatus = 401;

            // Assert
            Assert.AreEqual(1, order.idOrder);
            Assert.AreEqual(101, order.idProduct);
            Assert.AreEqual(201, order.idClient);
            Assert.AreEqual(301, order.idManager);
            Assert.IsNotNull(order.date);
            Assert.AreEqual(401, order.idStatus);
        }
    }
}
