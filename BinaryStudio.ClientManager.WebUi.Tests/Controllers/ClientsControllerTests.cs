using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using NSubstitute;
using FizzWare.NBuilder;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class ClientsControllerTests
    {
        private Person[] clients;

        [SetUp]
        public void CreateClientsList()
        {
            
            /*  clients = new[]
                {
                    new Person
                        {
                            CreationDate = new DateTime(2010, 1, 27),
                            Email = "client1@mail.ru",
                            FirstName = "Peter",
                            LastName = "Petrov",
                            RoleValue = (int)PersonRole.Client,
                            Id = 3
                        },
                    new Person
                        {
                            CreationDate = new DateTime(2011, 9, 17),
                            Email = "client2@gmail.com",
                            FirstName = "Sidor",
                            LastName = "Sidorov",
                            RoleValue = (int)PersonRole.Employee,
                            Id = 7
                        },
                    new Person
                        {
                            CreationDate = new DateTime(2012, 6, 16),
                            Email = "client3@gmail.com",
                            FirstName = "Ivan",
                            LastName = "Ivanov",
                            RoleValue = (int)PersonRole.Client,
                            Id = 13
                        }
                */
        }

        private readonly MailMessage[] messages = {
                                                      new MailMessage()
                                                          {
                                                              Body = "This is body of first message",
                                                              Date = new DateTime(2010, 1, 27),
                                                              Id = 3,
                                                              Receivers = new Collection<Person>(),
                                                              Sender = new Person()
                                                                           {
                                                                               CreationDate = new DateTime(2010, 1, 27),
                                                                               Email = "client1@mail.ru",
                                                                               FirstName = "Peter",
                                                                               LastName = "Petrov",
                                                                               RoleValue = (int)PersonRole.Client,
                                                                               Id = 3
                                                                           },
                                                              Subject = "Mail_1"
                                                          },
                                                      new MailMessage()
                                                          {
                                                              Body = "This is body of second message",
                                                              Date = new DateTime(2010, 1, 27),
                                                              Id = 4,
                                                              Receivers = new Collection<Person>(),
                                                              Sender = new Person()
                                                                           {
                                                                               CreationDate = new DateTime(2010, 1, 27),
                                                                               Email = "client1@mail.ru",
                                                                               FirstName = "Peter",
                                                                               LastName = "Petrov",
                                                                               RoleValue = (int)PersonRole.Client,
                                                                               Id = 3
                                                                           },
                                                              Subject = "Mail_2"
                                                          },
                                                      new MailMessage()
                                                          {
                                                              Body = "This is body of third message",
                                                              Date = new DateTime(2010, 1, 27),
                                                              Id = 5,
                                                              Receivers = new Collection<Person>(),
                                                              Sender = new Person()
                                                                           {
                                                                               CreationDate = new DateTime(2010, 1, 27),
                                                                               Email = "ivan@mail.ru",
                                                                               FirstName = "Ivan",
                                                                               LastName = "Ivanov",
                                                                               RoleValue = (int)PersonRole.Client,
                                                                               Id = 1
                                                                           },
                                                              Subject = "Mail_3"
                                                          }
                                                  };

        [Test]
        public void Should_Return300_WhenFillsListWith300Employees()
        {
            /*var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Person>()).Returns(clients.AsQueryable());
            var clientController = new ClientsController(mock.Object);

            var viewResult = clientController.Index();
            var viewResultModel = viewResult.Model as IEnumerable<Person>;

            Assert.IsNotNull(viewResultModel.SingleOrDefault(x => x.Id == 3));
            Assert.IsNull(viewResultModel.SingleOrDefault(x => x.Id == 7));
            Assert.IsNotNull(viewResultModel.SingleOrDefault(x => x.Id == 13));*/
            
            //arrange
            var data = Builder<Person>.CreateListOfSize(1000)
                .All()
                .With(x => x.Role = PersonRole.Employee)
                .Random(300)
                .With(x => x.Role = PersonRole.Client)
                .Random(700)
                .Build().AsQueryable();

            var repository = Substitute.For<IRepository>();
            repository.Query<Person>().Returns(data);

            //act
            var model = (IEnumerable<Person>) new ClientsController(repository).Index().Model;

            //assert
            model.Count().Should().Be(300);
        }

        [Test]
        public void ShouldNot_RaiseAnException_WhenClientsListIsEmpty()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Person>()).Returns(new Person[0].AsQueryable());
            var clientController = new ClientsController(mock.Object);

            Assert.DoesNotThrow(() => clientController.Index());
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void ShoudNot_RaiseException_WhenMessageListIsEmpty(int id)
        {
            //arrange
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<MailMessage>()).
                Returns(new MailMessage[0].AsQueryable());

            //act
            var clientController = new ClientsController(mock.Object);

            //assert
            Assert.DoesNotThrow(() => clientController.MailingHistory(id));

        }

        [Test]
        [TestCase(3)]
        [TestCase(1)]
        public void Shoud_ReturnResultOnlyMessageWithClientId_WhenMailHistoryIsRequested(int id)
        {
            //arrange
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<MailMessage>()).
                Returns(messages.AsQueryable());
            
            //act
            var clientController = new ClientsController(mock.Object);
            var resultView = clientController.MailingHistory(id);
            var resultModel = resultView.Model as IEnumerable<MailMessage>;

            //assert
            foreach (var message in resultModel)
            {
                Assert.That(message.Sender.Id, Is.EqualTo(id));
            }
        }
    }
}
