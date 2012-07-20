using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.WebUi.Models;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using FizzWare.NBuilder.Generators;
using Moq;
using NUnit.Framework;
using FluentAssertions;


namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class InquiryControllerTests
    {
        private IList<Inquiry> inquiries;
        
        [SetUp]
        public void GenerateInquiriesList()
        {
            Clock.FreezedTime = new DateTime(2012, 7, 19);

            inquiries = Builder<Inquiry>.CreateListOfSize(40)
                .All()
                .With(x => x.Client = Builder<Person>.CreateNew().Build())
                .With(x => x.Assignee = Builder<Person>.CreateNew().Build())
                .With(x => x.Source = Builder<MailMessage>.CreateNew().Build())
                .TheFirst(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(January.The1st, January.The31st))
                .TheNext(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(February.The15th, February.The28th))
                .TheNext(1)
                .With(x => x.ReferenceDate = new DateTime(DateTime.Now.Year, 3, 1))
                .TheNext(9)
                .With(x => x.ReferenceDate = GetRandom.DateTime(March.The1st, March.The31st))
                .TheNext(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(Clock.Now.GetStartOfBusinessWeek(),
                    Clock.Now.GetEndOfBusinessWeek()))
                .Build();
        }

        [Test]
        public void Should_ReturnFullListOfInquiries_WhenRequested()
        {
            // arrange
            var mock = new Mock<IRepository>();
            mock.Setup(z => z.Query<Inquiry>(x => x.Client, x => x.Source)).Returns(inquiries.AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);

            // act
            var response = inquiriesController.Index();
            var list = response.Model as IEnumerable<Inquiry>;

            // assert
            list.Count().Should().Be(40);
        }

        [Test]
        public void ShouldNot_RaiseException_WhenReposioryIsEmpty()
        {
            // arrange
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>()).Returns(new List<Inquiry>().AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);

            // act
            var response = inquiriesController.Index();

            // assert
            Assert.IsNotNull(response);
        }

        [Test]
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(8)]
        public void Should_ReturnInquiryWithSpecifiedId_WhenIsDetailsRequested(int id)
        {
            //setup
            var inquiry = Builder<Inquiry>.CreateNew()
                .With(x => x.Id = id)
                .Build();

            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Get<Inquiry>(id, z => z.Client, z => z.Source, z => z.Source.Sender)).Returns(inquiry);
            var inquiriesController = new InquiriesController(mock.Object);
            
            //act
            var result = inquiriesController.Details(id).Model as Inquiry;
            
            //assert
            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test]
        public void Should_ReturnListOfInquiriesForCurrentBusinessWeekAndFullListOfEmployees_WhenRequested()
        {
            // arrange
            var mock = new Mock<IRepository>();
            mock.Setup(z => z.Query<Inquiry>(x => x.Client)).Returns(inquiries.AsQueryable());
            mock.Setup(x => x.Query<Person>()).Returns(
                Builder<Person>.CreateListOfSize(10)
                .All()
                .With(x => x.Role = PersonRole.Client)
                .Random(7)
                .With(x => x.Role = PersonRole.Employee)
                .Build()
                .AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);

            // act
            var viewModel = inquiriesController.Week().Model as WeekViewModel;
            var inquiriesList = viewModel.Days;

            // assert
            var inquiriesCount = 0;
            foreach (var day in inquiriesList)
            {
                inquiriesCount += day.Inquiries.Count();
            }

            inquiriesCount.Should().Be(10);

            var employeesList = viewModel.Employees;
            employeesList.Count.Should().Be(7);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Should_ReturnListOfInquiriesForCurrentMonth_WhenRequested(int month)
        {
            // arrange
            Clock.FreezedTime = new DateTime(DateTime.Now.Year, month, 10);
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>(z => z.Client)).Returns(inquiries.AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);
            var viewResult = inquiriesController.Month().Model as MonthViewModel;
            var inquiriesList = viewResult.Days;
            var inquiriesCount = 0;

            var i = 0;

            foreach (var day in inquiriesList)
            {
                inquiriesCount += day.Inquiries.Count();
                i++;
            }
            inquiriesCount.Should().Be(10);
        }
    }
}
