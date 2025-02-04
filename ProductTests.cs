using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    [TestFixture]
    public class ProductTests
    {
        private Product productsTest1;
        private Product productsTest2;
        private DbContextOptions<StoreSampleContext> options;

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<StoreSampleContext>()
                        .UseInMemoryDatabase(databaseName: "temp_ProductsDB").Options;

            productsTest1 = new Product()
            {
                Productid = 1,
                Productname = "Laptop Dell XPS 15"
            };
            productsTest2 = new Product()
            {
                Productid = 2,
                Productname = "MacBook Pro 16"
            };

            using (var context = new StoreSampleContext(options))
            {
                context.Products.AddRange(productsTest1, productsTest2);
                context.SaveChanges();
            }
        }

        [Test]
        [Order(1)]
        public void ProductsRepositorio_ObtenerTodos_DeberiaRetornarListaProductos()
        {
            using (var context = new StoreSampleContext(options))
            {
                //Act:
                var productRepositorio = new ProductsRepository(context);
                var productslist = productRepositorio.GetAll().ToList();

                //Assert:
                Assert.That(productslist, Is.Not.Null);
                Assert.That(productslist.Count, Is.EqualTo(2));
                Assert.That(productslist.Select(p => p.Productname), Is.EquivalentTo(new[] { "Laptop Dell XPS 15", "MacBook Pro 16" }));
            }
        }

        [Test]
        [Order(2)]
        public void ProductsController_GetAllProducts_ObtenerListaProducts()
        {
            //Arrange:
            var products = new List<ProductsDTO>()
            {
                new ProductsDTO() { Productid = 1, Productname = "Producto1"},
                new ProductsDTO() {Productid = 2, Productname = "Producto2"},
                new ProductsDTO() {Productid = 3, Productname = "Producto3"},
            };

            var mockProductssServices = new Mock<IProductsServices>();
            mockProductssServices.Setup(x => x.GetAllProducts()).Returns(products);

            var productController = new ProductsController(mockProductssServices.Object);
            var actionResult = productController.GetProducts();
            var result = actionResult.Result as OkObjectResult;
            var productsDB = result.Value as IEnumerable<ProductsDTO>;

            //Assert
            CollectionAssert.AreEqual(products, productsDB);
            Assert.That(products.Count, Is.EqualTo(products.Count()));

        }
    }
}
