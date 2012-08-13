using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.ClientManager.DomainModel.Input;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    public class MailMessageParserFactoryTests
    {
        [Test]
        public void Should_ReturnMailMessageThunderbirdParser_WhenUserAgentContainsThunderBird()
        {
            //arrange
            var userAgent = "\r\nUser-Agent: Mozilla/5.0 (Windows NT 6.1; rv:14.0) Gecko/20120713 Thunderbird/14.0\r\n";
            var mailMessageParserFactory = new MailMessageParserFactory();

            //act
            var result=mailMessageParserFactory.GetMailMessageParser(userAgent) as MailMessageParserThunderbird;

            //assert
            result.Should().NotBeNull();
        }
    }
}
