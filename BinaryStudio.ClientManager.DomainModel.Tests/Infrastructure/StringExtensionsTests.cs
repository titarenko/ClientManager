using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Infrastructure
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void Should_ReturnTrue_WhenRequestedIsNullOrEmptyInStringEqualNull()
        {
            //arrange
            string testString = null;

            //act
            var result=testString.IsNullOrEmpty();

            //assert
            result.Should().BeTrue();
        }

        [Test]
        public void Should_ReturnTrue_WhenRequestedIsNullOrEmptyInEmptyString()
        {
            //arrange
            string testString = "";

            //act
            var result = testString.IsNullOrEmpty();

            //assert
            result.Should().BeTrue();
        }

        [Test]
        public void Should_ReturnTrue_WhenRequestedIsNullOrEmptyInStringWithOnlySpaces()
        {
            //arrange
            string testString = "    ";

            //act
            var result = testString.IsNullOrEmpty();

            //assert
            result.Should().BeTrue();
        }

        [Test]
        public void Should_ReturnFalse_WhenRequestedIsNullOrEmptyInStringWithNotEmptyString()
        {
            //arrange
            string testString = "   a   ";

            //act
            var result = testString.IsNullOrEmpty();

            //assert
            result.Should().BeFalse();
        }

        [Test]
        public void Should_ReturnNormalString_WhenRequestedOrInNotEmptyString()
        {
            //arrange
            string testString = "Not Empty String";

            //act
            var result = testString.Or("default string");

            //assert
            result.Should().Be("Not Empty String");
        }

        [Test, TestCaseSource("Should_ReturnDefaultString_WhenRequestedOrInEmptyOrNullString_TestCaseSource")]
        public void Should_ReturnDefaultString_WhenRequestedOrInEmptyOrNullString(string testString)
        {
            //act
            var result=testString.Or("default string");

            //assert
            result.Should().Be("default string");
        }

        public IEnumerable<TestCaseData> Should_ReturnDefaultString_WhenRequestedOrInEmptyOrNullString_TestCaseSource()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData("");
            yield return new TestCaseData("     ");
        }

        [Test]
        public void Should_ReturnCuttenString_WhenCutRequestedForStringWithLength9AndMaximalLength7()
        {
            //arrange
            var testString = "123456789";

            //act
            var result = testString.Cut(7);

            //assert
            result.Should().Be("1234...");
        }

        [Test]
        public void Should_ReturnFullString_WhenCutRequestedForStringWithLength9AndMaximalLength9And12()
        {
            //arrange
            var testString = "123456789";

            //act
            var result = testString.Cut(9);

            //assert
            result.Should().Be("123456789");
        }

        [Test]
        public void Should_ReturnFormattedString_WhenCalledFillWith2IntegerParameters()
        {
            //arrange
            var format = "{0}: lala {1}";

            //act
            var result = format.Fill(2, 3);

            //assert
            result.Should().Be("2: lala 3");
        }
    }
}
