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

        [Test, TestCaseSource("Should_ReturnMailAddressFromBody_WnehCalledGetReceiversFromForwardedMail_TestCaseSource")]
        public void Should_ReturnMailAddressFromBody_WnehCalledGetReceiversFromForwardedMail(string body, List<MailAddress> mailAddress)
        {
            //arrange
            var mailMessage = new MailMessage
            {
                Body = body,
            };
            var mailMessageParser = new MailMessageParser();

            //act
            var result = mailMessageParser.GetReceivers(mailMessage);

            //assert
            result.Should().Contain(mailAddress);
            result.Count.Should().Be(mailAddress.Count);
        }

        public IEnumerable<TestCaseData> Should_ReturnMailAddressFromBody_WnehCalledGetReceiversFromForwardedMail_TestCaseSource()
        {  
            //Outlook 2010
            yield return new TestCaseData(
                "From: Ivan Zaporozhchenko [mailto:1van1111@mail.ru] \n Sent: Thursday, August 09, 2012 1:30 PM \n To: 1van1111@i.ua; Ivan Zaporozhchenko \n Cc: studiobinary@gmail.com \n Subject: AAAAAAAAAAAAAAAAAAAAAAAA!!!!!!!!!!!\n \n asdfdgfdvcdx vds f",
                new List<MailAddress>
                    {
                        new MailAddress("1van1111@i.ua")
                    });
            yield return new TestCaseData(
                "From: Ivan Zaporozhchenko [mailto:1van1111@mail.ru] \n Sent: Friday, August 10, 2012 12:31 PM \n To: 1van1111@mail.ru; 1van1111@i.ua; clientmanagertest@yandex.ru \n Subject: Hi \n \n HIIIIIIII!!",
                new List<MailAddress>
                    {
                        new MailAddress("1van1111@mail.ru"),
                        new MailAddress("1van1111@i.ua"),
                        new MailAddress("clientmanagertest@yandex.ru"),
                    });
            //gmail web ui
            yield return new TestCaseData(
                "---------- Forwarded message ----------\nFrom: Ivan Zaporozhchenko <1van1111@mail.ru>\nDate: 2012/8/9\nSubject: AAAAAAAAAAAAAAAAAAAAAAAA!!!!!!!!!!!\nTo: 1van1111@i.ua, Ivan Zaporozhchenko <1van1111@mail.ru>\nCc: \"studiobinary@gmail.com\" <studiobinary@gmail.com>\n\n\n asdfdgfdvcdx vds f",
                new List<MailAddress>
                    {
                        new MailAddress("1van1111@i.ua"),
                        new MailAddress("1van1111@mail.ru"),
                    });
        }
    }
}
