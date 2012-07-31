using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.DomainModel.Input;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    public class AeEmailClientIntegrationTests
    {
        [Test, Ignore("Set up password before executing this integration test.")]
        public void ShouldNot_ThrowException_WhenTryingToConnect()
        {
            // arrange
            var configuration = new Mock<IConfiguration>();
            var settings = new Dictionary<string, string>
            {
                {"Host", "imap.yandex.ua"},
                {"Port", "993"},
                {"Username", "cman2012@yandex.ua"},
                {"Password", ""}, // put password here
                {"Secure", "true"},
                {"SkipSslValidation", "true"}
            };

            configuration
                .Setup(x => x.GetSubsection(It.Is<string>(y => y == "EmailClient")))
                .Returns(configuration.Object);
            configuration
                .Setup(x => x.GetValue(It.IsAny<string>()))
                .Returns((string x) => settings[x]);
            configuration
                .Setup(x => x.GetValue<int>(It.IsAny<string>()))
                .Returns((string x) => int.Parse(settings[x]));
            configuration
                .Setup(x => x.GetValue<bool>(It.IsAny<string>()))
                .Returns((string x) => bool.Parse(settings[x]));

            // act
            //using (new AeEmailClient(configuration.Object))
            //{
            //}

            // assert
            Assert.Pass();
        }
    }
}