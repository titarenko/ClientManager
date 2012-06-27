using System;
using System.Threading;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture, Category("Integration")]
    class TimerTests
    {
        private TimeSpan timeSpan;

        [SetUp]
        public void SetUp()
        {
            timeSpan = new TimeSpan(0, 0, 0, 2);
        }

        [Test]
        public void ShouldNot_RaiseOnTickEvent_WhenTimerIsDisabled()
        {
            // arrange
            var isRaised = false;
            var timer = new DomainModel.Infrastructure.Timer();
            timer.OnTick += (sender, args) => isRaised = true;
            timer.Interval = timeSpan;

            // act 
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);

            // check
            Assert.That(!isRaised);
        }

        [Test]
        public void Should_RaiseOnTickEvent_WhenTimeIsGone()
        {
            // arrange
            var isRaised = false;
            var timer = new DomainModel.Infrastructure.Timer();
            timer.OnTick += (sender, args) => isRaised = true;
            timer.Interval = timeSpan;

            // act
            timer.Enabled = true;
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);

            // check
            Assert.That(isRaised);
        }
    }
}
