using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Input;
using FizzWare.NBuilder;
using NSubstitute;
using NUnit.Framework;
using MailMessage = BinaryStudio.ClientManager.DomainModel.Entities.MailMessage;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class InquiryFactoryTests
    {
        [Test]
        public void Should_CreateInquiryWithCorrectFields_WhenCreateInquiryCalled()
        {
            // arrange
            var employee = Builder<Person>.CreateNew()
                .With(x=>x.Role==PersonRole.Employee)
                .Build();
            var mailMessage = Builder<MailMessage>.CreateNew()
                .With(z => z.Sender = Builder<Person>.CreateNew().Build())
                .With(z=>z.Receivers=new List<Person>{employee})
                .Build();
            var repository = Substitute.For<IRepository>();
            repository.Query<User>().ReturnsForAnyArgs(new List<User>
                {
                    new User
                    {
                        RelatedPerson = employee,
                        CurrentTeam = new Team
                                          {
                                              Name = "1"
                                          }
                    }
                }.AsQueryable());

            var inquiryFactory = new InquiryFactory(repository);

            // act
            var inquiry = inquiryFactory.CreateInquiry(mailMessage);

            // assert
            Assert.That(
                inquiry.Description == mailMessage.Body &&
                inquiry.ReferenceDate == null &&
                inquiry.Client == mailMessage.Sender &&
                inquiry.Subject == mailMessage.Subject &&
                inquiry.Source == mailMessage &&
                inquiry.Owner.Name=="1");
        }
    }
}
