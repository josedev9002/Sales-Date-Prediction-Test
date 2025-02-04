using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Sales_Date_Prediction_.Controllers;
using Sales_Date_Prediction_.DTO_s;
using Sales_Date_Prediction_.Interfaces;
using Sales_Date_Prediction_.Models;
using Sales_Date_Prediction_.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesDatePrediction_Test
{
    public class OrdersTests
    {
        private Order ordersTest1;
        private Order ordersTest2;
        private DbContextOptions<StoreSampleContext> options;

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<StoreSampleContext>()
                        .UseInMemoryDatabase(databaseName: "temp_OrdersDB").Options;

            ordersTest1 = new Order()
            {
                Custid = 1,
                Empid = 1,
                Orderid = 1,
                Shipaddress = "",
                Shipcity = "",
                Shipcountry = "",
                Shipname = ""
            };
            ordersTest2 = new Order()
            {
                Custid = 2,
                Empid = 2,
                Orderid = 2,
                Shipaddress = "",
                Shipcity = "",
                Shipcountry = "",
                Shipname = ""
            };

            using (var context = new StoreSampleContext(options))
            {
                context.Orders.AddRange(ordersTest1, ordersTest2);
                context.SaveChanges();
            }
        }

        [Test]
        [Order(1)]
        public void OrdersRepositorio_ObtenerTodos_RetornarListaOrdenesByCustId()
        {
            using (var context = new StoreSampleContext(options))
            {
                //Act:
                var orderRepositorio = new OrdersRepository(context);
                var orderslist = orderRepositorio.GetOrdersByCustId(1).ToList();

                //Assert:
                Assert.That(orderslist, Is.Not.Null);
                Assert.That(orderslist.Count, Is.EqualTo(1));
                Assert.That(orderslist.Select(p => p.Orderid), Is.EquivalentTo(new[] { 1 }));
            }
        }

        [Test]
        [Order(2)]
        public void OrdersController_GetAllOrders_ObtenerListaOrdenes()
        {
            //Arrange:
            var orders = new List<OrdersDTO>()
            {
                new OrdersDTO() { Custid = 1, Empid = 1, Orderid = 1, Shipaddress = "", Shipcity = "",  Shipcountry = "", Shipname = ""},
                new OrdersDTO() {Custid = 2, Empid = 2, Orderid = 2,  Shipaddress = "", Shipcity = "",  Shipcountry = "", Shipname = ""},
                new OrdersDTO() {Custid = 3, Empid = 3, Orderid = 3, Shipaddress = "", Shipcity = "",  Shipcountry = "", Shipname = ""},
            };
            var mockOrdersServices = new Mock<IOrderServices>();
            mockOrdersServices.Setup(x => x.GetOrdersByCustId(1)).Returns(orders);

            var ordersController = new OrdersController(mockOrdersServices.Object);
            var actionResult = ordersController.GetOrderByCustId(1);
            var result = actionResult.Result as OkObjectResult;
            var ordersDB = result.Value as IEnumerable<OrdersDTO>;

            //Assert
            CollectionAssert.AreEqual(orders, ordersDB);
            Assert.That(orders.Count, Is.EqualTo(orders.Count()));

        }
    }
}
