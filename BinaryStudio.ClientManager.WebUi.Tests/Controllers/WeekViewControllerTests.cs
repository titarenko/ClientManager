using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Models;
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
        public void Should_ReturnSortedByDateListOfInquiriesForCurrentWeek_WhenIndexRequested()
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
            var viewModel = weekViewController.Week().Model as WeekViewModel;

            // assert
            Assert.That(viewModel.Inquiries.Count==5);
            Assert.That(viewModel.Inquiries[0].ReferenceDate <= viewModel.Inquiries[1].ReferenceDate);
            Assert.That(viewModel.Inquiries[1].ReferenceDate <= viewModel.Inquiries[2].ReferenceDate);
            Assert.That(viewModel.Inquiries[2].ReferenceDate <= viewModel.Inquiries[3].ReferenceDate);
            Assert.That(viewModel.Inquiries[3].ReferenceDate <= viewModel.Inquiries[4].ReferenceDate);
        }

        [Test]
        public void ShouldNot_RaiseAnException_WhenIndexRequestedAndRepositoryIsEmpty()
        {
            //arrange
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>(z => z.Client, z => z.Source, z => z.Assignee))
                .Returns(new List<Inquiry>().AsQueryable());
            var weekViewController = new WeekViewController(mock.Object);

            //act and assert 
            Assert.DoesNotThrow(() => weekViewController.Week());
        }

        [Test]
        public void Should_Return10PersonsWithEmployeeRole_WhenIndexRequestedWith10Employees()
        {
            //arrange
            var persons = Builder<Person>.CreateListOfSize(50)
                .All()
                .With(x => x.Role = PersonRole.Client)
                .Random(10)
                .With(x => x.Role = PersonRole.Employee)
                .Build();

            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Person>(z => z.RelatedMails)).Returns(persons.AsQueryable());
            var weekViewController = new WeekViewController(mock.Object);

            //act
            var viewModel = weekViewController.Week().Model as WeekViewModel;

            //assert
            Assert.That(viewModel.Employees.Count==10);
        }
    }
}
