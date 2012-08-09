using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.DomainModel.Input;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using MailMessage = BinaryStudio.ClientManager.DomainModel.Input.MailMessage;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class MailMessagePersisterTests
    {
        private IRepository repository=Substitute.For<IRepository>() ;
        private MailMessagePersister mailMessagePersister;
        private IEmailClient aeEmailClient = Substitute.For<IEmailClient>();

        [SetUp]
        public void Initializer()
        {
            var inquiryFactory = new InquiryFactory();
            mailMessagePersister = new MailMessagePersister(repository,aeEmailClient,inquiryFactory);
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
                Receivers = new List<MailAddress> { receiver1, receiver2 }
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
                Role = (int)PersonRole.Employee,
                CreationDate = new DateTime(2000, 1, 1),
                FirstName = "Employee",
                LastName = "Kozlov",
                Email = "employee2@gmail.com"
            };

            repository.Query<Person>().ReturnsForAnyArgs(new List<Person> { expectedPerson, expectedReceiver1, expectedReceiver2 }.AsQueryable());

            //act
            var result = mailMessagePersister.Convert(mailMessage);

            //assert
            result.Body.Should().Be("This is Body");
            result.Subject.Should().Be("This is Subject");
            result.Date.Should().Be(new DateTime(2012, 1, 1));
            result.Sender.Should().Be(expectedPerson);
            result.Receivers.Should().Contain(expectedReceiver1);
            result.Receivers.Should().Contain(expectedReceiver2);
            result.Receivers.Count.Should().Be(2);
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
            repository.Query<Person>().Returns(new List<Person>().AsQueryable());

            //act
            var result = mailMessagePersister.Convert(mailMessage);

            //assert
            repository.Received().Save(addingClient);
            repository.Received().Save(addingEmployee);
        }

        [Test]
        public void Should_ReturnMailMessageWith_WhenCallingConvertWithForwardedEmail()
        {
            //arrange
            var mailMessage = new DomainModel.Input.MailMessage
                                  {
                                      Subject = "FW: Subject",
                                      Sender = new MailAddress("employee@gmail.com"),
                                      Body = "some text... from: client@gmail.com To:..."
                                  };

            repository.Query<Person>().Returns(new List<Person>{
                    new Person
                        {
                            Email = "client@gmail.com"
                        },
                    new Person
                        {
                            Email = "employee@gmail.com"
                        }
                }.AsQueryable());

            //act
            var result = mailMessagePersister.Convert(mailMessage);

            //assert
            result.Sender.Email.Should().Be("client@gmail.com");
            result.Receivers.Any(x => x.Email == "employee@gmail.com").Should().BeTrue();
        }

        [Test]
        public void ShouldNot_ReturnNullInSenderAndEmptyCollectionInReceiverFields_WhenCallingConvertMethodWhenSenderAndReceiverIsNotExistInRepository()
        {
            //arrange
            var receiver = new MailAddress("employee@gmail.com");
            var mailMessage = new DomainModel.Input.MailMessage
                                  {
                                      Body = "",
                                      Subject = "",
                                      Date = new DateTime(2000, 1, 1),
                                      Receivers = new List<MailAddress> { receiver },
                                      Sender = new MailAddress("client@gmail.com")
                                  };
            repository.Query<Person>().Returns(new List<Person>().AsQueryable());

            //act
            var result = mailMessagePersister.Convert(mailMessage);

            //assert
            result.Sender.Should().NotBeNull();
            result.Receivers.Should().NotBeEmpty();
        }

        [Test]
        public void Should_SaveMailMessageToRepository_WhenEmployeeSendMessageToClientWithCarbonCopyToOurSystem()
        {
            //arrange
            aeEmailClient.GetUnreadMessages().Returns(new List<MailMessage>
                {
                    new MailMessage
                    {
                        Body = "a",
                        Date = Clock.Now,
                        Subject = "s",
                        Sender = new MailAddress("employee@gmail.com","employee"),
                        Receivers = new List<MailAddress>{new MailAddress("client@gmail.com","client")},
                    }
                });
            repository.Query<Person>().ReturnsForAnyArgs(new List<Person>
                {
                    new Person
                        {
                            Email = "employee@gmail.com",
                            FirstName = "employee",
                            Role = PersonRole.Employee
                        },
                    new Person
                        {
                            Email="client@gmail.com",
                            FirstName = "client",
                            Role = PersonRole.Client
                        }
                }.AsQueryable());
            

            //act
            mailMessagePersister.Proceed(aeEmailClient, EventArgs.Empty);
 
            //assert
            repository.Received().Save(Arg.Is<Entities.MailMessage>(x => x.Sender.Email == "employee@gmail.com"
                && x.Receivers.Any(receiver => receiver.Email == "client@gmail.com")
                && x.Receivers.Count == 1));
        }
    }
}
