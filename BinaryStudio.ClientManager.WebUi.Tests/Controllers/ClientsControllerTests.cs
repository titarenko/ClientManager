using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using NUnit.Framework;
using Moq;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class ClientsControllerTests
    {
        private readonly Person[] clients =
                {
                    new Person()
                        {
                            CreationDate = new DateTime(2010, 1, 27),
                            Email = "client1@mail.ru",
                            FirstName = "Peter",
                            LastName = "Petrov",
                            Role = PersonRole.Client,
                            Id = 3
                        },
                    new Person()
                        {
                            CreationDate = new DateTime(2011, 9, 17),
                            Email = "client2@gmail.com",
                            FirstName = "Sidor",
                            LastName = "Sidorov",
                            Role = PersonRole.Client,
                            Id = 7
                        },
                    new Person()
                        {
                            CreationDate = new DateTime(2012, 6, 16),
                            Email = "client3@gmail.com",
                            FirstName = "Ivan",
                            LastName = "Ivanov",
                            Role = PersonRole.Client,
                            Id = 13
                        }
                };

        [Test]
        public void Should_ReturnClientsList_WhenRequested()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Person>(client => client.Role == PersonRole.Client)).
                Returns(clients.AsQueryable());
            var clientController = new ClientsController(mock.Object);

            var viewResult = clientController.Clients();
            var viewResultModel = viewResult.Model as IEnumerable<Person>;

            Assert.IsNotNull(viewResultModel.SingleOrDefault(x => x.Id == 3));
            Assert.IsNotNull(viewResultModel.SingleOrDefault(x => x.Id == 7));
            Assert.IsNotNull(viewResultModel.SingleOrDefault(x => x.Id == 13));
        }

        [Test]
        public void ShouldNot_RaiseAnException_WhenClientsListIsEmpty()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Person>(client => client.Role == PersonRole.Client)).
                Returns(new Person[0].AsQueryable());
            var clientController = new ClientsController(mock.Object);

            Assert.DoesNotThrow(() => clientController.Clients());
        }
    }
}
