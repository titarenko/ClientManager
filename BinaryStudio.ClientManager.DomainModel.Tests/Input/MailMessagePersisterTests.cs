using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.DomainModel.Input;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using MailMessage = BinaryStudio.ClientManager.DomainModel.Input.MailMessage;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class MailMessagePersisterTests
    {
        private IRepository repository = Substitute.For<IRepository>();
        private MailMessagePersister mailMessagePersister;
        private IEmailClient aeEmailClient = Substitute.For<IEmailClient>();

        [SetUp]
        public void Initializer()
        {
            mailMessagePersister = new MailMessagePersister(repository, aeEmailClient, new InquiryFactory(), new MailMessageParser());
        }

        [Test]
        public void Should_ReturnMailMessageWithRightData_WhenCallingConvertForMailWithExistingClientAndEmloyees()
        {
            //arrange 
            var receiver1 = new MailAddress("employee@gmail.com", "Employee Petrov");
            var receiver2 = new MailAddress("employee2@gmail.com", "Employee Kozlov");

            var mailMessage = Builder<MailMessage>.CreateNew()
                .With(x => x.Date = GetRandom.DateTime())
                .With(x => x.Sender = new MailAddress("client@gmail.com", "Client Ivanov"))
                .With(x => x.Receivers = new List<MailAddress> {receiver1, receiver2})
                .Build();

            var persons = Builder<Person>.CreateListOfSize(3)
                .All().With(x => x.CreationDate = GetRandom.DateTime())
                .TheFirst(1)
                .With(x => x.Role = PersonRole.Client)
                .With(x => x.FirstName = "Client")
                .With(x => x.LastName = "Ivanov")
                .With(x => x.Email = "client@gmail.com")
                .TheNext(1)
                .With(x => x.Role = PersonRole.Employee)
                .With(x => x.FirstName = "Employee")
                .With(x => x.LastName = "Petrov")
                .With(x => x.Email = "employee@gmail.com")
                .TheNext(1)
                .With(x => x.Role = PersonRole.Employee)
                .With(x => x.FirstName = "Employee")
                .With(x => x.LastName = "Kozlov")
                .With(x => x.Email = "employee2@gmail.com")
                .Build();

            repository.Query<Person>().ReturnsForAnyArgs(persons.AsQueryable());

            //act
            var result = mailMessagePersister.Convert(mailMessage);

            //assert
            Assert.That(result.Body == mailMessage.Body &&
                result.Subject == mailMessage.Subject &&
                result.Date == mailMessage.Date &&
                result.Sender == persons[0] &&
                result.Receivers.Count == 2 &&
                result.Receivers.Contains(persons[1]) &&
                result.Receivers.Contains(persons[2]));
        }

        [Test]
        public void Should_CallSaveMethodOfRepository_WhenCallingConvertForMailWithUnknownClientAndEmployee()
        {
            //arrange
            var mailMessage = Builder<MailMessage>.CreateNew()
                .With(x => x.Sender =
                    new MailAddress("client@gmail.com", "Client 1"))
                .With(x => x.Receivers =
                    new List<MailAddress> { new MailAddress("employee@gmail.com", "Employee 1") })
                .Build();

            var client = new Person
            {
                CreationDate = mailMessage.Date,
                FirstName = "Client",
                LastName = "1",
                Email = "client@gmail.com"
            };

            var employee = new Person
            {
                CreationDate = mailMessage.Date,
                FirstName = "Employee",
                LastName = "1",
                Email = "employee@gmail.com"
            };

            repository.Query<Person>().Returns(new List<Person>().AsQueryable());

            //act
            mailMessagePersister.Convert(mailMessage);

            //assert
            repository.Received().Save(client);
            repository.Received().Save(employee);
        }

        [Test]
        public void Should_ReturnMailMessageWithRightSenderAndReceivers_WhenCallingConvertWithForwardedEmail()
        {
            //arrange
            var mailMessage = new DomainModel.Input.MailMessage
            {
                Subject = "FW: Subject",
                Sender = new MailAddress("employee@gmail.com"),
                Body = "some text... from: client@gmail.com \nTo: employee@gmail.com \n....."
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
            mailMessagePersister.ProcessMessage(aeEmailClient, EventArgs.Empty);
 
            //assert
            repository.Received().Save(Arg.Is<Entities.MailMessage>(x => x.Sender.Email == "employee@gmail.com"
                && x.Receivers.Any(receiver => receiver.Email == "client@gmail.com")
                && x.Receivers.Count == 1));
        }

        [Test]
        public void Should_SaveMailMessageToRepositoryAndCreateAndSaveInquiry_WhenClientSendMessageToEmployeeWithCarbonCopyToOurSystem()
        {
            //arrange
            var mailMessage = new MailMessage
                {
                    Body = "a",
                    Date = Clock.Now,
                    Subject = "s",
                    Sender = new MailAddress("client@gmail.com", "client"),
                    Receivers =
                        new List<MailAddress> {new MailAddress("employee@gmail.com", "employee")},
                };
            aeEmailClient.GetUnreadMessages().Returns(new List<MailMessage>
                {
                    mailMessage
                }.AsQueryable());
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
            repository.Query<Inquiry>().ReturnsForAnyArgs(new List<Inquiry>().AsQueryable());

            //act
            mailMessagePersister.ProcessMessage(aeEmailClient, EventArgs.Empty);

            //assert
            repository.Received().Save(Arg.Is<Entities.MailMessage>(x => x.Sender.Email == "client@gmail.com"
                && x.Receivers.Any(receiver => receiver.Email == "employee@gmail.com")
                && x.Receivers.Count == 1));
            repository.Received().Save(Arg.Is<Inquiry>(x=>x.Subject=="s" &&
                x.ReferenceDate==null &&
                x.Client.Email=="client@gmail.com" && 
                x.Client.FirstName=="client" &&
                x.Client.Role==PersonRole.Client));
        }
    }
}
