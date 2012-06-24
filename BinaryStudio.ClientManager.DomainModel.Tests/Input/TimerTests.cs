using System;
using System.Threading;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture, Category("Integration")]
    class TimerTests
    {
        private TimeSpan _timeSpan;

        [SetUp]
        public void SetUp()
        {
            _timeSpan = new TimeSpan(0, 0, 0, 2);
        }

        [Test]
        public void ShouldNot_RaiseOnTickEvent_WhenTimerIsDisabled()
        {
            // arrange
            var isRaised = false;
            var timer = new DomainModel.Infrastructure.Timer();
            timer.OnTick += (sender, args) => isRaised = true;
            timer.Interval = _timeSpan;

            // act 
            Thread.Sleep(_timeSpan);
            Thread.Sleep(_timeSpan);
            Thread.Sleep(_timeSpan);

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
            timer.Interval = _timeSpan;

            // act
            timer.Enabled = true;
            Thread.Sleep(_timeSpan);
            Thread.Sleep(_timeSpan);
            Thread.Sleep(_timeSpan);

            // check
            Assert.That(isRaised);
        }
    }
}
