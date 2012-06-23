using System;
using System.Threading;
using NUnit.Framework;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class ThreadingTimerTests
    {
        [Test]
        public void ShouldNot_RaiseOnTickEvent_WhenTimerDisable()
        {
            // arrange
            var timer = new ThreadingTimer();
            var isRaised = false;
            timer.OnTick += (sender, args) =>  isRaised = true;
            var timeSpan = new TimeSpan(0, 0, 0, 1);
            timer.Interval = timeSpan;

            // act & check
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);

            Assert.That(!isRaised);
        }

        [Test]
        public void Should_RaiseOnTickEvent_WhenTimeIsGone()
        {
            // arrange
            var timer = new ThreadingTimer();
            var isRaised = false;
            timer.OnTick += (sender, args) => isRaised = true;
            var timeSpan = new TimeSpan(0, 0, 0, 2);
            timer.Interval = timeSpan;

            // check 
            Assert.That(!isRaised);

            timer.Enabled = true;
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);

            // check
            Assert.That(isRaised);

            isRaised = false;
            timer.Enabled = false;
            timer.Interval = new TimeSpan(0, 0, 0, 1);

            // check
            Assert.That(!isRaised);

            timer.Enabled = true;
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);

            // check
            Assert.That(isRaised);

            isRaised = false;
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);
            Thread.Sleep(timeSpan);

            // check
            Assert.That(isRaised);
        }
    }
}
