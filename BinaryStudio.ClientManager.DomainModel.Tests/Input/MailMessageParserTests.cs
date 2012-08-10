using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using BinaryStudio.ClientManager.DomainModel.Input;
using MailMessage = BinaryStudio.ClientManager.DomainModel.Input.MailMessage;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class MailMessageParserTests
    {
        [Test, TestCaseSource("Should_ReturnMailAddressFromBody_WnehCalledGetSenderFromForwardedMail_TestCaseSource")]
        public void Should_ReturnMailAddressFromBody_WnehCalledGetSenderFromForwardedMail(string body, string mailAddress)
        {
            //arrange
            var mailMessage = new MailMessage
                                  {
                                      Body = body,
                                  };
            var mailMessageParser = new MailMessageParser();

            //act
            var result = mailMessageParser.GetSenderFromForwardedMail(mailMessage);

            //assert
            result.Address.Should().Be(mailAddress);
        }

        public IEnumerable<TestCaseData> Should_ReturnMailAddressFromBody_WnehCalledGetSenderFromForwardedMail_TestCaseSource()
        {
            yield return  new TestCaseData("olololo From: Ivan Zaporozhchenko [mailto:1van1111@mail.ru] Sent: Thursday, August 09, 2012 1:30 PM To: 1van1111@i.ua; Ivan Zaporozhchenko Cc: studiobinary@gmail.com Subject: AAAAAAAAAAAAAAAAAAAAAAAA!!!!!!!!!!! asdfdgfdvcdx vds f","1van1111@mail.ru");
        }
    }
}
