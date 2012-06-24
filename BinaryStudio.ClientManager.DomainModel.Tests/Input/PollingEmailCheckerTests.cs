using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Input;
using BinaryStudio.ClientManager.DomainModel.Tests.Infrastructure;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    public class PollingEmailCheckerTests
    {
        private FakeTimer timer;
        private Mock<IEmailClient> clientMock;

        [SetUp]
        public void SetUp()
        {
            timer = new FakeTimer();
            clientMock = new Mock<IEmailClient>();
        }

        [Test]
        [TestCaseSource("EmailReceivedEventTestDataSource")]
        public void EmailReceivedEventTest(
            string description,
            IList<DomainModel.Input.MailMessage> messagesReturnedFromServer, 
            bool shouldRaiseReceived)
        {
            // arrange
            clientMock.Setup(x => x.GetUnreadMessages()).Returns(messagesReturnedFromServer);

            var checker = new PollingEmailChecker(timer, clientMock.Object);

            var received = false;
            checker.EmailReceived += (sender, args) => received = true;

            // act
            timer.RaiseOnTick();

            // assert
            Assert.AreEqual(shouldRaiseReceived, received, description);
        }

        [Test]
        public void ShouldNot_ThrowNullReferenceException_WhenNoHandlersExistForEmailReceived()
        {
            // arrange
            clientMock.Setup(x => x.GetUnreadMessages()).Returns(new DomainModel.Input.MailMessage[10]);

            new PollingEmailChecker(timer, clientMock.Object);

            // act
            timer.RaiseOnTick();

            // assert
            Assert.Pass();
        }

        public IEnumerable<TestCaseData> EmailReceivedEventTestDataSource()
        {
            yield return new TestCaseData(
                "Should Not Raise Email Received Event When No Messages Are Received From Server", 
                new DomainModel.Input.MailMessage[0], 
                false);

            yield return new TestCaseData(
                "Should Raise Email Received Event When Messages Are Received From Server",
                new DomainModel.Input.MailMessage[5], 
                true);
        }
    }
}