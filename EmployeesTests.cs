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
    public class EmployeesTests
    {
        private IConfiguration _configuration;
        private DbContextOptions<StoreSampleContext> _options;
        private EmployeesRepository _employyesRepository;

        [SetUp]
        public void SetUp()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IConfiguration configuration = configBuilder; 
            _employyesRepository = new EmployeesRepository(configuration);
        }

        [Test]
        [Order(1)]
        public void EmployeesRepositorio_ObtenerTodos_DeberiaRetornarListaEmpleados()
        {
            //Act: 
            var employeeslist = _employyesRepository.GetAll().ToList();

            //Assert: 
            Assert.That(employeeslist, Is.Not.Null);
            Assert.That(employeeslist.Count, Is.GreaterThan(0));
        }

        [Test]
        [Order(2)]
        public void EmployesController_GetAllEmployees_ObtenerListaEmployees()
        {
            //Arrange:
            var employees = new List<EmployeesDTO>()
            {
                new EmployeesDTO() {Empid = "1", FullName = "Juan Perez"},
                new EmployeesDTO() {Empid = "2", FullName = "Pedro Perez"},
                new EmployeesDTO() {Empid = "3", FullName = "Compay Segundo"},
            };

            var mockEmployeesServices = new Mock<IEmployeesServices>();
            mockEmployeesServices.Setup(x => x.GetAllEmployees()).Returns(employees);

            var employyesController = new EmployeesController(mockEmployeesServices.Object);
            var actionResult = employyesController.GetAllEmployees();
            var result = actionResult.Result as OkObjectResult;
            var employesDB = result.Value as IEnumerable<EmployeesDTO>;

            //Assert
            CollectionAssert.AreEqual(employees, employesDB);
            Assert.That(employees.Count, Is.EqualTo(employees.Count()));

        }
    }
}
