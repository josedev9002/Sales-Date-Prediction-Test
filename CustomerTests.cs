using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class CustomerTests
    {
          
        private IConfiguration _configuration;
        private DbContextOptions<StoreSampleContext> _options;
        private CustomerRepository _customerRepository;

        [SetUp]
        public void SetUp()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IConfiguration configuration = configBuilder;
            _customerRepository = new CustomerRepository(configuration);
        }

        [Test]
        [Order(1)]
        public void CustomerRepositorio_ObtenerTodos_DeberiaRetornarListaCustomerDatePrediction()
        {
            //Act:
            var customerlist = _customerRepository.GetAll().ToList();

            //Assert:
            Assert.That(customerlist, Is.Not.Null);
            Assert.That(customerlist.Count, Is.GreaterThan(0));
        }

        [Test]
        [Order(2)]
        public void CustomerController_GetAllCustomer_ObtenerListaClientes()
        {
            //Arrange:
            var customers = new List<CustomerDatePredictionDTO>()
            {
                new CustomerDatePredictionDTO() { CustomerName = "Juan Perez", LastOrderDate = DateTime.Now, NextPredictedOrder = DateTime.Now.AddDays(10)},
                new CustomerDatePredictionDTO() { CustomerName = "Pedro Perez", LastOrderDate = DateTime.Now, NextPredictedOrder = DateTime.Now.AddDays(20)},
                new CustomerDatePredictionDTO() { CustomerName = "Compay Segundo", LastOrderDate = DateTime.Now, NextPredictedOrder = DateTime.Now.AddDays(30)},
            };

            var mockCustomerServices = new Mock<ICustomerServices>();
            mockCustomerServices.Setup(x => x.GetCustomerDatePrediction()).Returns(customers);

            var customersController = new CustomersController(mockCustomerServices.Object);
            var actionResult = customersController.GetCustomerDatePredictions();
            var result = actionResult.Result as OkObjectResult;
            var customersDB = result.Value as IEnumerable<CustomerDatePredictionDTO>;

            //Assert
            CollectionAssert.AreEqual(customers, customersDB);
            Assert.That(customers.Count, Is.EqualTo(customers.Count()));

        }
    }
}

