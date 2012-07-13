using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.WebUi.Models;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using FizzWare.NBuilder.Generators;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class WeekViewControllerTests
    {
        [Test]
        public void Should_ReturnSortedByDateListOfInquiriesForCurrentWeek_WhenRequested()
        {
            // arrange
            var inquiries = Builder<Inquiry>.CreateListOfSize(10)
                .All()
                .With(x => x.ReferenceDate = GetRandom.DateTime(January.The1st, DateTime.Now))
                .Random(5)
                .With(x => x.ReferenceDate = GetRandom.DateTime(new DateTime(2012, 7, 9),
                                                                new DateTime(2012, 7, 13)))
                .Build();

            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>(z => z.Client, z => z.Source, z => z.Assignee))
                .Returns(inquiries.AsQueryable());
            var weekViewController = new WeekViewController(mock.Object);

            // act
            var viewResult = weekViewController.Index().Model as WeekViewModel;

            // assert
            Assert.That(viewResult.Inquiries.Count==5);
            Assert.That(viewResult.Inquiries[0].ReferenceDate <= viewResult.Inquiries[1].ReferenceDate);
            Assert.That(viewResult.Inquiries[1].ReferenceDate <= viewResult.Inquiries[2].ReferenceDate);
            Assert.That(viewResult.Inquiries[2].ReferenceDate <= viewResult.Inquiries[3].ReferenceDate);
            Assert.That(viewResult.Inquiries[3].ReferenceDate <= viewResult.Inquiries[4].ReferenceDate);
        }

        [Test]
        public void ShouldNot_RaiseAnException_WhenWeekViewRequestedAndRepositoryIsEmpty()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>(z => z.Client, z => z.Source, z => z.Assignee))
                .Returns(new List<Inquiry>().AsQueryable());

            var weekViewController = new WeekViewController(mock.Object);

            Assert.DoesNotThrow(() => weekViewController.Index());
        }
    }
}
