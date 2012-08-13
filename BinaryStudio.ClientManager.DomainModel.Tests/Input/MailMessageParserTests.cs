using System;
using System.Collections.Generic;
using System.Net.Mail;
using FluentAssertions;
using NUnit.Framework;
using BinaryStudio.ClientManager.DomainModel.Input;
using MailMessage = BinaryStudio.ClientManager.DomainModel.Input.MailMessage;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class MailMessageParserTests
    {
        [Test, TestCaseSource("Should_ReturnSenderMailAddressFromBody_WnehCalledGetSenderFromForwardedMail_TestCaseSource")]
        public void Should_ReturnSenderMailAddressFromBody_WnehCalledGetSenderFromForwardedMail(string body, string mailAddress)
        {
            //arrange
            var mailMessage = new MailMessage
            {
                Body = body,
            };
            var mailMessageParser = new MailMessageParserThunderbird();

            //act
            var result = mailMessageParser.GetSender(mailMessage);

            //assert
            result.Address.Should().Be(mailAddress);
        }

        public IEnumerable<TestCaseData> Should_ReturnSenderMailAddressFromBody_WnehCalledGetSender_TestCaseSource()
        {
            yield return  new TestCaseData("olololo From: Ivan Zaporozhchenko [mailto:1van1111@mail.ru] Sent: Thursday, August 09, 2012 1:30 PM To: 1van1111@i.ua; Ivan Zaporozhchenko Cc: studiobinary@gmail.com Subject: AAAAAAAAAAAAAAAAAAAAAAAA!!!!!!!!!!! asdfdgfdvcdx vds f","1van1111@mail.ru");
        }

        [Test]
        public void Should_ThrowException_WhenCalledGetSenderWithNoEmailAddressInBody()
        {
            //arrange
            var mailMessage = new MailMessage
                                  {
                                      Body = "No email addresses there"
                                  };
            var mailMessageParser = new MailMessageParserThunderbird();

            //act
            Action action = () => mailMessageParser.GetSender(mailMessage);

            //assert
            action
                .ShouldThrow<ApplicationException>()
                .WithMessage("Forwarded message has illegal format");
        }

        [Test, TestCaseSource("Should_ReturnBodyFromOriginalMessage_WnehCalledGetBody_TestCaseSource")]
        public void Should_ReturnBodyFromOriginalMessage_WnehCalledGetBody(string body, string originalBody)
        {
            //arrange
            var mailMessage = new MailMessage
            {
                Body = body,
            };
            var mailMessageParser = new MailMessageParserThunderbird();

            //act
            var result = mailMessageParser.GetBody(mailMessage);

            //assert
            result.Should().Be(originalBody);
        }

        public IEnumerable<TestCaseData> Should_ReturnBodyFromOriginalMessage_WnehCalledGetBody_TestCaseSource()
        {       
             yield return new TestCaseData(
                "-------- Original Message --------\r\nSubject:\tAAAAAAAAAAAAAAAAAAAAAAAA!!!!!!!!!!!\r\nDate:\tThu, 09 Aug 2012 14:30:00 +0400\r\nFrom:\tIvan Zaporozhchenko <1van1111@mail.ru>\r\nReply-To:\tIvan Zaporozhchenko <1van1111@mail.ru>\r\nTo:\t1van1111@i.ua, Ivan Zaporozhchenko <1van1111@mail.ru>\r\nCC:\t<studiobinary@gmail.com>\r\n\r\noriginal body",
               "\r\noriginal body");
            }

        [Test, TestCaseSource("Should_ReturnRightSubject_WhenCalledGetSubject_TestCaseSource")]
        public void Should_ReturnRightSubject_WhenCalledGetSubject(string subject)
        {
            var parser = new MailMessageParserThunderbird();
            var parsedSubject = parser.GetSubject(subject);
            parsedSubject.Should().Be("Test subject");
        }

        public IEnumerable<TestCaseData> Should_ReturnRightSubject_WhenCalledGetSubject_TestCaseSource()
        {
            yield return new TestCaseData("Fwd: Test subject");
            yield return new TestCaseData("Fw: Test subject");
            yield return new TestCaseData("fwd: Test subject");
            yield return new TestCaseData("fw: Test subject");
            yield return new TestCaseData("Test subject");
        }
    }
}
