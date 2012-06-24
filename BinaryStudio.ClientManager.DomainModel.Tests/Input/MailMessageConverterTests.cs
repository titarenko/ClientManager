using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using BinaryStudio.ClientManager.DomainModel;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.DomainModel.Input;
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
            var converter = new MailMessageConverter();
            var receiver = new MailAddress("employee@gmail.com", "Employee Petrov");
            var mailMessage = new DomainModel.Input.MailMessage
                                  {
                                      Body = "This is Body",
                                      Date = new DateTime(2012, 1, 1),
                                      Subject = "This is Subject",
                                      Sender = new MailAddress("client@gmail.com", "Client Ivanov"),
                                      Receivers=new List<MailAddress> {receiver}
                                  };
            var expectedReceiver = new Person
                                       {
                                           FirstName = "Employee",
                                           LastName = "Petrov",
                                           Email = "employee@gmail.com"
                                       };
            var personComparer = new PersonComparer();

            //act
            var result=converter.ConvertMailMessageFromInputTypeToEntityType(mailMessage);

            //assert
            Assert.AreEqual("This is Body", result.Body);
            Assert.AreEqual("This is Subject", result.Subject);
            Assert.AreEqual(new DateTime(2012, 1, 1), result.Date);
            Assert.AreEqual("Client", result.Sender.FirstName);
            Assert.AreEqual("Ivanov", result.Sender.LastName);
            Assert.AreEqual("client@gmail.com",result.Sender.Email);
            Assert.That(result.Receivers.Contains(expectedReceiver,personComparer));
        }
    }
}
