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
        public void Should_CallSaveMethodOfRepositoryWithInquiry_WhenCallCreateInquiry()
        {
            //arrange
            var mailMessage = Builder<MailMessage>.CreateNew()
                .With(z => z.Sender = Builder<Person>.CreateNew()
                                          .With(y => y.Email = "person@example.com")
                                          .With(y=>y.Role=PersonRole.Client)
                                          .Build())
                .With(z => z.Subject = "Need 2 C++ developers")
                .With(z => z.Body = "Some body")
                .Build(); 

            var repository = Substitute.For<IRepository>();
            repository.Query<Inquiry>().ReturnsForAnyArgs(
                new List<Inquiry>{new Inquiry
                    {
                        Source = new MailMessage()
                    }
                }.AsQueryable());
            var inquiryFactory = new InquiryFactory();

            //act
            inquiryFactory.CreateInquiry(mailMessage);

            //assert
            repository.Received().Save(Arg.Is<Inquiry>(x => x.Description == mailMessage.Body &&
                x.ReferenceDate == null &&
                x.Client == mailMessage.Sender &&
                x.Subject == mailMessage.Subject &&
                x.Source == mailMessage));
        }
    }
}
