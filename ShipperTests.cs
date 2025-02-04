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
    public class ShipperTests
    {
        private Shipper ShipperTest1;
        private Shipper ShipperTest2;
        private DbContextOptions<StoreSampleContext> options;

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<StoreSampleContext>()
                        .UseInMemoryDatabase(databaseName: "temp_ShipperDB").Options;

            ShipperTest1 = new Shipper()
            {
                Shipperid = 1,
                Companyname = "Compañia1",
                Phone = "123456"

            };
            ShipperTest2 = new Shipper()
            {
                Shipperid = 2,
                Companyname = "Compañia2",
                Phone = "123456"
            };

            using (var context = new StoreSampleContext(options))
            {
                context.Shippers.AddRange(ShipperTest1, ShipperTest2);
                context.SaveChanges();
            }
        }

        [Test]
        [Order(1)]
        public void ShippersRepositorio_ObtenerShipper_DeberiaRetornarListaTransportistas()
        {
            using (var context = new StoreSampleContext(options))
            {
                //Act:
                var shipperRepositorio = new ShippersRepository(context);
                var shipperlist = shipperRepositorio.GetAll().ToList();
                //Assert: 
                Assert.Equals(shipperlist, Is.Not.Null);
                Assert.That(shipperlist.Count, Is.EqualTo(2));
                Assert.That(shipperlist.Select(p => p.Companyname), Is.EquivalentTo(new[] { "Compañia1", "Compañia2" }));
            }
        }
        
        [Test]
        [Order(2)]
        public void ShipperController_GetAllShipper_ObtenerListaShipper()
        {
            //Arrange:
            var Shippers = new List<ShippersDTO>()
            {
                new ShippersDTO() {ShipperID = "1", CompanyName = "Compañia1"},
                new ShippersDTO() {ShipperID = "2", CompanyName = "Compañia2"},
                new ShippersDTO() {ShipperID = "3", CompanyName = "Compañia3"},
            };

            var mockShippersServices = new Mock<IShippersServices>();
            mockShippersServices.Setup(x => x.GetAllShippers()).Returns(Shippers);
            
            var shipperController = new ShippersController(mockShippersServices.Object);
            var actionResult = shipperController.GetAllShippers();
            var result = actionResult.Result as OkObjectResult;
            var shippersDB = result.Value as IEnumerable<ShippersDTO>;

            //Assert
            CollectionAssert.AreEqual(Shippers, shippersDB);
            Assert.That(Shippers.Count, Is.EqualTo(Shippers.Count()));

        }

    }
}
