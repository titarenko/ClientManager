using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.DomainModel.Input;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture, Ignore("Integration test, not intended to run after each build.")]
    public class AeEmailClientIntegrationTests
    {
        [Test]
        public void ShouldNot_ThrowException_WhenTryingToConnect()
        {
            // arrange
            var configuration = new Mock<IConfiguration>();
            var settings = new Dictionary<string, string>
            {
                {"Host", "imap.gmail.com"},
                {"Port", "993"},
                {"Username", "studiobinary@gmail.com"},
                {"Password", "binarytest"},
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
            using (new AeEmailClient(configuration.Object))
            {
            }

            // assert
            Assert.Pass();
        }
    }
}