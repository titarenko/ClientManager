using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Infrastructure
{
    [TestFixture]
    public class SafeExtensionsTests
    {
        [Test]
        public void Should_ReturnNormalResult_WhenRequstedSafeGetForInquiriesInTagEntityIfTagIsNotNull()
        {
            //arrange
            var tag = new Tag
                          {
                              Inquiries = new List<Inquiry>
                                              {
                                                  new Inquiry()
                                              }
                          };
                         

            //act
            var result = tag.SafeGet(x => x.Inquiries);

            //assert
            result.Count().Should().Be(1);
        }

        [Test]
        public void Should_ReturnNull_WhenRequestedSafeGetForNameInTagEntityIfTagIsNull()
        {
            //arrange
            Tag tag = null;

            //act
            var result = tag.SafeGet(x => x.Name);

            //assert
            Assert.Pass();
            result.Should().Be(null);
        }
    }
}
