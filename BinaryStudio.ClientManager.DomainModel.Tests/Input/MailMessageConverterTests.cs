using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Input;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class MailMessageConverterTests
    {
        private Mock<IRepository> mock;
        private MailMessageConverter converter;

        [SetUp]
        public void Initializer()
        {
            mock = new Mock<IRepository>();
            converter = new MailMessageConverter(mock.Object);
        }

        [Test]
        public void Should_ReturnMailMessageWithRightData_WhenCallingConvertMethod()
        {
            //arrange 
            var receiver1 = new MailAddress("employee@gmail.com", "Employee Petrov");
            var receiver2 = new MailAddress("employee2@gmail.com", "Employee Kozlov");

            var mailMessage = new DomainModel.Input.MailMessage
            {
                Body = "This is Body",
                Date = new DateTime(2012, 1, 1),
                Subject = "This is Subject",
                Sender = new MailAddress("client@gmail.com", "Client Ivanov"),
                Receivers=new List<MailAddress> {receiver1,receiver2}
            };

            var expectedPerson = new Person
            {
                Id = 1,
                Role = PersonRole.Client,
                CreationDate = new DateTime(2000, 1, 1),
                FirstName = "Client",
                LastName = "Ivanov",
                Email = "client@gmail.com",
            };

            var expectedReceiver1 = new Person
            {
                Id = 2,
                Role = PersonRole.Employee,
                CreationDate = new DateTime(2000, 1, 1),
                FirstName = "Employee",
                LastName = "Petrov",
                Email = "employee@gmail.com"
            };

            var expectedReceiver2 = new Person
            {
                Id = 3,
                Role = PersonRole.Employee,
                CreationDate = new DateTime(2000, 1, 1),
                FirstName = "Employee",
                LastName = "Kozlov",
                Email = "employee2@gmail.com"
            };

            mock.Setup(it => it.Query<Person>()).Returns(new List<Person> { expectedPerson, expectedReceiver1, expectedReceiver2 }.AsQueryable());

            //act
            var result=converter.Convert(mailMessage);

            //assert
            Assert.AreEqual("This is Body", result.Body);
            Assert.AreEqual("This is Subject", result.Subject);
            Assert.AreEqual(new DateTime(2012, 1, 1), result.Date);
            Assert.AreEqual(expectedPerson, result.Sender);
            CollectionAssert.Contains(result.Receivers, expectedReceiver1);
            CollectionAssert.Contains(result.Receivers, expectedReceiver2);
            Assert.AreEqual(2,result.Receivers.Count);
        }

        [Test]
        public void Should_CallSaveMethodOfRepositoryObjectForClientAndEmployee_WhenCallingConvertWithUnknownYetMailAddressesOfClientAndEmployee()
        {
            //arrange
            var receiver = new MailAddress("employee@gmail.com", "Employee 1");
            var mailMessage = new DomainModel.Input.MailMessage
            {
                Body = "Body",
                Date = new DateTime(2012, 1, 1),
                Subject = "Subject",
                Sender = new MailAddress("client@gmail.com", "Client 1"),
                Receivers = new List<MailAddress> { receiver }
            };

            var addingClient = new Person
            {   
                CreationDate = mailMessage.Date,
                FirstName = "Client",
                LastName = "1",
                Email = "client@gmail.com"
            };

            var addingEmployee = new Person
            {
                CreationDate = mailMessage.Date,
                FirstName = "Employee",
                LastName = "1",
                Email = "employee@gmail.com"
            };

            //act
            var result=converter.Convert(mailMessage);

            //assert
            mock.Verify(x =>x.Save(addingClient), Times.Once());
            mock.Verify(x=>x.Save(addingEmployee), Times.Once());
        }

        [Test]
        public void ShouldNot_ReturnNullInSenderAndEmptyCollectionInReceiverFields_WhenCallingConvertMethodWhenSenderAndReceiverIsNotExistInRepository()
        {
            //arrange
            var receiver = new MailAddress("employee@gmail.com");
            var mailMessage = new DomainModel.Input.MailMessage
                                  {
                                      Body = "",
                                      Subject="",
                                      Date = new DateTime(2000,1,1),
                                      Receivers = new List<MailAddress>{receiver},
                                      Sender = new MailAddress("client@gmail.com")
                                  };

            //act;
            var result = converter.Convert(mailMessage);

            //assert
            Assert.IsNotNull(result.Sender);
            CollectionAssert.IsNotEmpty(result.Receivers);
        }
    }
}
