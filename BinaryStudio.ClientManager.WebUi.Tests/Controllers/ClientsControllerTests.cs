using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
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
                    Builder<MailMessage>.
                    CreateListOfSize(10).
                    All().
                    With(d => d.Date = new RandomGenerator().DateTime()).
                    Build()).
                Build();

            var repository = Substitute.For<IRepository>();
            repository.Get<Person>(7777).Returns(person);

            var clientController = new ClientsController(repository);

            // act
            var viewResult = clientController.MailingHistory(7777).Model as Person;

            // assert
            for (int i = 1; i < viewResult.RelatedMails.Count; i++)
                Assert.That(viewResult.RelatedMails[i].Date >=
                    viewResult.RelatedMails[i - 1].Date);
        }
    }
}