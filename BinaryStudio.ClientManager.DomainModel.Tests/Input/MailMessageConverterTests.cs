using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using BinaryStudio.ClientManager.DomainModel;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
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
        public void Should_ReturnMailMessageWithRightData_WhenCallingConvertMailMessageFromInputTypeToEntityType()
        {
            //arrange 
            var receiver = new MailAddress("employee@gmail.com", "Employee Petrov");
            var mailMessage = new DomainModel.Input.MailMessage
            {
                Body = "This is Body",
                Date = new DateTime(2012, 1, 1),
                Subject = "This is Subject",
                Sender = new MailAddress("client@gmail.com", "Client Ivanov"),
                Receivers=new List<MailAddress> {receiver}
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
            var expectedReceiver = new Person
            {
                Id = 2,
                Role = PersonRole.Employee,
                CreationDate = new DateTime(2000,1,1),
                FirstName = "Employee",
                LastName = "Petrov",
                Email = "employee@gmail.com"
            };
            var returnValues = new[] {new List<Person> {expectedPerson}, new List<Person> {expectedReceiver}};
            int numCalls = 0;
            mock.Setup(it => it.Query<Person>()).Returns(() => returnValues[numCalls].AsQueryable()).Callback(() => numCalls++);

            //act
            var result=converter.ConvertMailMessageFromInputTypeToEntityType(mailMessage);

            //assert
            Assert.AreEqual("This is Body", result.Body);
            Assert.AreEqual("This is Subject", result.Subject);
            Assert.AreEqual(new DateTime(2012, 1, 1), result.Date);
            Assert.AreEqual(expectedPerson, result.Sender);
            Assert.That(result.Receivers.Contains(expectedReceiver));
            Assert.AreEqual(1,result.Receivers.Count);
        }

        [Test]
        public void Should_CallSaveMethodOfRepositoryObjectTwice_WhenCallingConvertMailMessageFromInputTypeToEntityTypeWithUnknownYetMailAddressesOfClientAndEmployee()
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
            var result=converter.ConvertMailMessageFromInputTypeToEntityType(mailMessage);

            //assert
            mock.Verify(x =>x.Save(addingClient), Times.Exactly(1));
            mock.Verify(x=>x.Save(addingEmployee), Times.Exactly(1));
        }
    }
}
