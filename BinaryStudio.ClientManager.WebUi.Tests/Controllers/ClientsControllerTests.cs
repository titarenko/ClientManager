using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using NUnit.Framework;
using NSubstitute;
using FizzWare.NBuilder;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class ClientsControllerTests
    {
        private IList<Person> persons;

        [SetUp]
        public void CreatePersonsList()
        {
            persons = Builder<Person>.CreateListOfSize(1000)
                .All()
                .With(x => x.Role = PersonRole.Employee)
                .Random(300)
                .With(x => x.Role = PersonRole.Client)
                .Build();
        }

        [Test]
        public void Should_ReturnListOf300Persons_WhenListOfClientsRequested()
        {
            // arrange
            var repository = Substitute.For<IRepository>();
            repository.Query<Person>().Returns(persons.AsQueryable());

            // act
            var model = (IEnumerable<Person>)new ClientsController(repository).Index().Model;

            // assert
            model.Count().Should().Be(300);
        }

        [Test]
        public void ShouldNot_RaiseAnException_WhenClientsListIsEmpty()
        {
            // arrange
            var repository = Substitute.For<IRepository>();
            repository.Query<Person>().Returns(new List<Person>().AsQueryable());
            var clientController = new ClientsController(repository);

            // act & assert
            Assert.DoesNotThrow(() => clientController.Index());
        }

        [Test]
        public void Should_ReturnMailingHistoryOfSpecifiedPerson_WhenRequested()
        {
            // arrange
            var person = Builder<Person>.CreateNew()
                .With(x => x.Id = 7777).Build();

            var mails = Builder<MailMessage>.CreateListOfSize(25)
                .All().With(x => x.Date = GetRandom.DateTime())
                .With(x => x.Sender = Builder<Person>.CreateNew().Build())
                .With(x => x.Receivers = Builder<Person>.CreateListOfSize(3).Build())
                .TheFirst(7).With(x => x.Sender = person)
                .TheLast(7).With(x => x.Receivers = new List<Person> {person})
                .Build();

            var repository = Substitute.For<IRepository>();
            repository.Query<MailMessage>().ReturnsForAnyArgs(mails.AsQueryable());
            var clientController = new ClientsController(repository);

            // act
            var viewResult = (IList<MailMessage>)clientController.MailingHistory(7777).Model;

            // assert
            viewResult.Count().Should().Be(14);

            for (int i = 1; i < viewResult.Count; i++)
                viewResult[i].Date.Should().BeOnOrAfter(viewResult[i - 1].Date);
        }

        [Test]
        public void ShouldNot_ReturnNullAndShouldCallMethodGetOfIRepository_WhenRequestedDetails()
        {
            // arrange
            var returnedClient = new Person
            {
                Id = 1,
                Role=PersonRole.Client
            };
            var repository = Substitute.For<IRepository>();
            repository.Get<Person>(1).Returns(returnedClient);
            var clientController = new ClientsController(repository);

            // act 
            var viewModel = clientController.Details(1).Model as Person;

            // assert
            viewModel.Should().NotBeNull();
            repository.Received().Get<Person>(1);
        }

        [Test]
        public void ShouldNot_ReturnNullAnd_ShouldCallMethodGetOfIRepository_WhenRequestedEditWith1Parameter()
        {
            // arrange
            var returnedClient = new Person
            {
                Id = 3,
                Role = PersonRole.Client
            };
            var repository = Substitute.For<IRepository>();
            repository.Get<Person>(3).Returns(returnedClient);
            var clientController = new ClientsController(repository);

            // act 
            var viewModel = clientController.Edit(3).Model as Person;

            // assert
            viewModel.Should().NotBeNull();
            repository.Received().Get<Person>(3);
        }

        [Test]
        public void Should_GoToDetailsViewAndCallSaveMethodOfIRepository_WhenRequestedEditWith2Parameters()
        {
            // arrange
            var savedClient = new Person
            {
                Id = 1,
                Role = PersonRole.Client
            };
            var repository = Substitute.For<IRepository>();
            var clientController = new ClientsController(repository);

            // act
            var viewResult = clientController.Edit(1, savedClient);

            // assert
            viewResult.ViewName.Should().Be("Details");
            repository.Received().Save(savedClient);
        }
    }
}