using BinaryStudio.ClientManager.WebUi.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class TagViewModelTests
    {
        [Test]
        public void Should_ReturndotNet_WhenRequestedCssClassFromTagWithNameDotNet()
        {
            //arrange
            var tag = new TagViewModel
                {
                    Name = ".Net"
                };
            
            //act and assert
            tag.CssClass.Should().Be("dotnet");
        }

        [Test]
        public void Should_Returncplusplus_WhenRequestedCssClassFromTagWithNameCPlusPlus()
        {
            //arrange
            var tag = new TagViewModel
            {
                Name = "C++"
            };

            //act and assert
            tag.CssClass.Should().Be("cplusplus");
        }

        [Test]
        public void Should_Returnphp_WhenRequestedCssClassFromTagWithNamePhp()
        {
            //arrange
            var tag = new TagViewModel
            {
                Name = "Php"
            };

            //act and assert
            tag.CssClass.Should().Be("php");
        }
    }
}
