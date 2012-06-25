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
        [Test]
        public void Should_ReturnMailMessageWithRightData_WhenCallingConvertMailMessageFromInputTypeToEntityType()
        {
            //arrange
            var mock = new Mock<IRepository>();
            var converter = new MailMessageConverter(mock.Object);
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
            Expression<Func<Person, object>> expressionSenderEmailAddressShouldBeEqual = x => x.Email == mailMessage.Sender.Address;
            mock.Setup(x => x.Query(expressionSenderEmailAddressShouldBeEqual)).Returns();
            //act
            var result=converter.ConvertMailMessageFromInputTypeToEntityType(mailMessage);

            //assert
            mock.Verify(x => x.Query(expressionSenderEmailAddressShouldBeEqual), Times.Once());
            Assert.AreEqual("This is Body", result.Body);
            Assert.AreEqual("This is Subject", result.Subject);
            Assert.AreEqual(new DateTime(2012, 1, 1), result.Date);
            Assert.AreEqual("Client", result.Sender.FirstName);
            Assert.AreEqual("Ivanov", result.Sender.LastName);
            Assert.AreEqual("client@gmail.com",result.Sender.Email);
            Assert.That(result.Receivers.Contains(expectedReceiver));
        }
    }
}
