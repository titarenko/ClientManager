using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Controllers;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class EmployeesControllerTests
    {
        [Test]
        public void Should_ReturnListOf200Employees_WhenRequestedIndexWith200EmployeesExistsInRepository()
        {
            //arrange
            var data = Builder<Person>.CreateListOfSize(500)
                .All()
                .With(x => x.Role = PersonRole.Client)               
                .Random(200)
                .With(x => x.Role = PersonRole.Employee)
                .Build()
                .AsQueryable();

            var repository = Substitute.For<IRepository>();
            repository.Query<Person>().Returns(data);

            var employeesController = new EmployeesController(repository);

            //act
            var model = (IEnumerable<Person>)employeesController.Index().Model;

            //assert
            model.Count().Should().Be(200);
        }

        [Test]
        public void ShouldNot_RiseException_WhenListOfEmployeeIsEmpty()
        {
            //arrange
            var repository = Substitute.For<IRepository>();
            repository.Query<Person>().Returns(new List<Person>().AsQueryable());
            
            var employeesController = new EmployeesController(repository);

            //act
            employeesController.Index();

            //assert
            Assert.Pass();
        }

    }
}
