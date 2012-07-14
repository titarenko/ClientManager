using System;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Infrastructure
{
    [TestFixture]
    public class ClockTests
    {
        [Test]
        public void Should_ReturnFreezedTime_WhenItIsSetUp()
        {
            // arrange
            var freezedTime = new DateTime(2012, 1, 1);
            Clock.FreezedTime = freezedTime;

            // act & assert
            Clock.Now.Should().Be(freezedTime);
        }

        [Test]
        public void Should_BehaveLikeDateTime_WhenFreezedTimeIsNotSetUp()
        {
            // arrange
            Clock.FreezedTime = null;

            // act & assert
            DateTime.Now.Should().BeOnOrBefore(Clock.Now);
        }
    }
}