using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
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
        private IAppContext appContext=Substitute.For<IAppContext>();

        [SetUp]
        public void SetUp()
        {
            appContext.CurrentUser.Returns(new User());
        }

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

            var employeesController = new EmployeesController(repository, appContext);

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

            var employeesController = new EmployeesController(repository, appContext);

            //act
            employeesController.Index();

            //assert
            Assert.Pass();
        }

        [Test]
        public void ShouldNot_ReturnNullAndShouldCallMethodGetOfIRepository_WhenRequestedDetails()
        {
            //arrange
            var employee = new Person
            {
                Id = 1,
                Role = PersonRole.Employee
            };
            var repository = Substitute.For<IRepository>();
            repository.Get<Person>(1).Returns(employee);
            var employeeController = new EmployeesController(repository, appContext);


            //act 
            var viewModel = employeeController.Details(1).Model as Person;

            //assert
            viewModel.Should().NotBeNull();
            repository.Received().Get<Person>(1);
        }

        [Test]
        public void ShouldNot_ReturnNullAnd_ShouldCallMethodGetOfIRepository_WhenRequestedEditWith1Parameter()
        {
            //arrange
            var employee = new Person
            {
                Id = 3,
                Role = PersonRole.Employee
            };
            var repository = Substitute.For<IRepository>();
            repository.Get<Person>(3).Returns(employee);
            var employeeController = new EmployeesController(repository, appContext);


            //act 
            var viewModel = employeeController.Edit(3).Model as Person;

            //assert
            viewModel.Should().NotBeNull();
            repository.Received().Get<Person>(3);
        }

        [Test]
        public void Should_GoToDetailsViewAndCallSaveMethodOfIRepository_WhenRequestedEditWith2Parameters()
        {
            //arrange
            var employee = new Person
            {
                Id = 1,
                Role = PersonRole.Employee
            };
            var repository = Substitute.For<IRepository>();
            repository.Query<Person>().ReturnsForAnyArgs(new List<Person>{employee}.AsQueryable());
            var employeeController = new EmployeesController(repository, appContext);

            //act
            employeeController.Edit(1, employee);

            //act
            //viewResult.ViewName.Should().Be("Details");
            repository.Received().Save(employee);
        }

    }
}
