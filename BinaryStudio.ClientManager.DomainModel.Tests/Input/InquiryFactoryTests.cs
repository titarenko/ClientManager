using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Input;
using FizzWare.NBuilder;
using FluentAssertions;
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
                                          .With(y => y.Role = PersonRole.Client)
                                          .Build())
                .With(z => z.Subject = "Need 2 C++ developers")
                .With(z => z.Body = "Some body")
                .Build();

            var inquiryFactory = new InquiryFactory();

            //act
            var result = inquiryFactory.CreateInquiry(mailMessage);

            //assert
            result.Description.Should().Be(mailMessage.Body);
            result.ReferenceDate.Should().NotHaveValue();
            result.Client.Should().Be(mailMessage.Sender);
            result.Subject.Should().Be(mailMessage.Subject);
            result.Source.Should().Be(mailMessage);
        }
    }
}
