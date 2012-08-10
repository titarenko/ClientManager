using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Input;
using FizzWare.NBuilder;
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
            var mailMessage = Builder<MailMessage>.CreateNew()
                .With(z => z.Sender = Builder<Person>.CreateNew().Build())
                .Build();

            var inquiryFactory = new InquiryFactory();

            // act
            var inquiry = inquiryFactory.CreateInquiry(mailMessage);

            // assert
            Assert.That(
                inquiry.Description == mailMessage.Body &&
                inquiry.ReferenceDate == null &&
                inquiry.Client == mailMessage.Sender &&
                inquiry.Subject == mailMessage.Subject &&
                inquiry.Source == mailMessage);
        }
    }
}
