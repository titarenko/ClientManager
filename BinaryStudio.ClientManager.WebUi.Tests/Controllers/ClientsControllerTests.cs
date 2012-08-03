using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using Moq;
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
        public void Should_Return300_WhenFillsListWith300Clients()
        {
            //arrange
            var repository = Substitute.For<IRepository>();
            repository.Query<Person>().Returns(persons.AsQueryable());

            //act
            var model = (IEnumerable<Person>)new ClientsController(repository).Index().Model;

            //assert
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
        public void Should_ReturnSpecifiedPersonWithRelatedMailSortedByDate_WhenRequested()
        {
            // arrange
            var person = Builder<Person>.
                CreateNew().
                With(x => x.Id = 7777).
                With(x => x.RelatedMails =
                    Builder<MailMessage>
                    .CreateListOfSize(10)
                    .All()
                    .With(d => d.Id = 7777)
                    .With(d => d.Date = GetRandom.DateTime())
                    .With(d => d.Receivers = new List<Person>{x})
                    .Build())
               .Build();

            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Get<Person>(7777, z => z.RelatedMails)).Returns(person);
            mock.Setup(x => x.Get<MailMessage>(7777, z => z.Sender, z => z.Receivers)).Returns(person.RelatedMails.FirstOrDefault());
            var clientController = new ClientsController(mock.Object);

            // act
            var viewResult = clientController.MailingHistory(7777).Model as Person;

            // assert
            for (int i = 1; i < viewResult.RelatedMails.Count; i++)
                Assert.That(viewResult.RelatedMails[i].Date >=
                    viewResult.RelatedMails[i - 1].Date);
        }

        [Test]
        public void ShouldNot_ReturnNullAndShouldCallMethodGetOfIRepository_WhenRequestedDetails()
        {
            //arrange
            var returnedClient = new Person
                                     {
                                         Id = 1,
                                         Role=PersonRole.Client
                                     };
            var repository = Substitute.For<IRepository>();
            repository.Get<Person>(1).Returns(returnedClient);
            var clientController = new ClientsController(repository);
            

            //act 
            var viewModel = clientController.Details(1).Model as Person;

            //assert
            viewModel.Should().NotBeNull();
            repository.Received().Get<Person>(1);
        }

        [Test]
        public void ShouldNot_ReturnNullAnd_ShouldCallMethodGetOfIRepository_WhenRequestedEditWith1Parameter()
        {
            //arrange
            var returnedClient = new Person
            {
                Id = 3,
                Role = PersonRole.Client
            };
            var repository = Substitute.For<IRepository>();
            repository.Get<Person>(3).Returns(returnedClient);
            var clientController = new ClientsController(repository);


            //act 
            var viewModel = clientController.Edit(3).Model as Person;

            //assert
            viewModel.Should().NotBeNull();
            repository.Received().Get<Person>(3);
        }

        [Test]
        public void Should_GoToDetailsViewAndCallSaveMethodOfIRepository_WhenRequestedEditWith2Parameters()
        {
            //arrange
            var savedClient = new Person
                                  {
                                      Id = 1,
                                      Role = PersonRole.Client
                                  };
            var repository = Substitute.For<IRepository>();
            var clientController = new ClientsController(repository);

            //act
            var viewResult = clientController.Edit(1, savedClient);

            //act
            viewResult.ViewName.Should().Be("Details");
            repository.Received().Save(savedClient);
        }

    }
}