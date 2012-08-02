﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.WebUi.Infrastructure;
using BinaryStudio.ClientManager.WebUi.Models;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using FizzWare.NBuilder.Generators;
using Moq;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;
using UrlHelper = System.Web.Mvc.UrlHelper;


namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class InquiryControllerTests
    {
        private IList<Inquiry> inquiries;

        private IList<Tag> tags;
        
        [SetUp]
        public void GenerateInquiriesList()
        {
            Clock.FreezedTime = new DateTime(2012, 7, 19);

            tags = new List<Tag>
            {
                new Tag
                {
                    Id = 1,
                    Name = "tag1"
                },
                new Tag
                {
                    Id = 2,
                    Name = "tag2"
                }
            };

            inquiries = Builder<Inquiry>.CreateListOfSize(40)
                .All()
                .With(x => x.Client = Builder<Person>.CreateNew().Build())
                .With(x => x.Assignee = Builder<Person>.CreateNew().Build())
                .With(x => x.Source = Builder<MailMessage>.CreateNew().Build())
                .With(x => x.Tags = new List<Tag>())
                .TheFirst(10)
                .With(x => x.ReferenceDate = null)
                .With(x => x.Tags = new [] {tags[0]})
                .TheNext(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(February.The15th, February.The28th))
                .With(x => x.Tags = new [] {tags[1]})
                .TheNext(1)
                .With(x => x.ReferenceDate = new DateTime(Clock.Now.Year, 3, 1))
                .TheNext(9)
                .With(x => x.ReferenceDate = GetRandom.DateTime(March.The1st, March.The31st))
                .TheNext(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(
                    Clock.Now.GetStartOfBusinessWeek(), Clock.Now.GetEndOfBusinessWeek()))                
                .Build();

            tags[0].Inquiries = inquiries.Take(10).ToList();
            tags[1].Inquiries = inquiries.Skip(10).Take(10).ToList();
        }

        [Test]
        public void Should_ReturnFullListOfInquiries_WhenIndexRequested()
        {
            // arrange
            var mock = new Mock<IRepository>();
            mock.Setup(z => z.Query<Inquiry>(x => x.Client, x => x.Source)).Returns(inquiries.AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object, Substitute.For<IUrlHelper>());

            // act
            var response = inquiriesController.Index();
            var list = response.Model as IEnumerable<Inquiry>;

            // assert
            list.Count().Should().Be(40);
        }

        [Test]
        public void ShouldNot_RaiseException_WhenRepostioryIsEmptyAndIndexRequested()
        {
            // arrange
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>()).Returns(new List<Inquiry>().AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object, Substitute.For<IUrlHelper>());

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
            mock.Setup(z => z.Get<Inquiry>(id, x => x.Client, x => x.Source,
                x => x.Source.Sender, x => x.Comments, x => x.Assignee, x => x.Tags)).Returns(inquiry);
            var inquiriesController = new InquiriesController(mock.Object, Substitute.For<IUrlHelper>());
            
            //act
            var result = (Inquiry) inquiriesController.Details(id).Model;
            
            //assert
            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test]
        public void Should_ReturnListOfInquiriesForCurrentBusinessWeekAndFullListOfEmployees_WhenWeekRequested()
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
            var inquiriesController = new InquiriesController(mock.Object, Substitute.For<IUrlHelper>());

            // act
            var viewModel = inquiriesController.Week().Model as WeekViewModel;
            var inquiriesList = viewModel.Days;

            // assert
            var inquiriesCount = inquiriesList.Sum(day => day.Inquiries.Count());

            inquiriesCount.Should().Be(10);

            var employeesList = viewModel.Employees;
            employeesList.Count.Should().Be(7);
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        public void Should_ReturnListOfInquiriesForCurrentMonth_WhenMonthRequested(int month)
        {
            // arrange
            Clock.FreezedTime = new DateTime(Clock.Now.Year, month, 10);
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>(z => z.Client)).Returns(inquiries.AsQueryable());

            // act
            var inquiriesController = new InquiriesController(mock.Object, Substitute.For<IUrlHelper>());
            var viewResult = (MonthViewModel)inquiriesController.Month().Model;

            // assert
            Assert.That(viewResult.Weeks
                .All(x => x.Days
                    .Where(y => y.Inquiries.Any())
                    .All(z => z.Date.Month == month)));
        }
        
        [Test]
        public void Should_ReturnFullListOfInquiriesSortedByTag_WhenCalledAllFunction()
        {
            // arrange
            var repository = Substitute.For<IRepository>();
            repository.Query<Inquiry>().Returns(inquiries.AsQueryable());
            repository.Query<Tag>().Returns(tags.AsQueryable());

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action("Details", "Inquiries").Returns("awesomeUri");

            var inquiriesController = new InquiriesController(repository, urlHelper);

            // act
            var viewResult = (AllInquiriesViewModel) inquiriesController.All().Model;
            var categories = viewResult.Categories.ToList(); 

            // assert
            viewResult.InquiryDetailsUrl.Should().Be("awesomeUri");
            categories.Count.Should().Be(2);
            categories[1].Tag.Name.Should().Be("");
            categories[1].Inquiries.Count().Should().Be(20);
            categories[0].Tag.Name.Should().Be("tag2");
            categories[0].Inquiries.Count().Should().Be(10);
        }

        [Test]
        public void Should_ReturnSingleInquiryInEveryCategory_When_SeveralTagsAreAssigned_And_AllViewIsRequested()
        {
            // arrange
            var tags = Builder<Tag>.CreateListOfSize(2).Build();
            var inquiries = Builder<Inquiry>.CreateListOfSize(10)
                .All()
                .With(x => x.Client = Builder<Person>.CreateNew().Build())
                .With(x => x.ReferenceDate = July.The14th)
                .With(x => x.Tags = tags.Take(1).ToList())
                .TheLast(3)
                .With(x => x.Tags = tags)
                .Build();
            tags.ForEach(x => x.Inquiries = inquiries.Where(z => z.Tags.Any(y => y.Name == x.Name)).ToList());

            var repository = Substitute.For<IRepository>();
            repository.Query<Tag>().Returns(tags.AsQueryable());
            repository.Query<Inquiry>().Returns(inquiries.AsQueryable());

            var inquiriesController = new InquiriesController(repository, Substitute.For<IUrlHelper>());

            // act
            var viewResult = (AllInquiriesViewModel) inquiriesController.All().Model;
            var categories = viewResult.Categories.ToList();

            // assert
            categories.Count.Should().Be(2);

            categories[0].Tag.Name.Should().Be("Name1");
            categories[0].Inquiries.Count().Should().Be(10);

            categories[1].Tag.Name.Should().Be("Name2");
            categories[1].Inquiries.Count().Should().Be(3);

            categories[0].Inquiries.Should().Contain(x => x.Id == 9);
            categories[1].Inquiries.Should().Contain(x => x.Id == 9);

            categories[0].Inquiries.Should().Contain(x => x.Id == 1);
            categories[1].Inquiries.Should().NotContain(x => x.Id == 1);
        }

        [Test]
        public void ShouldNot_ReturnCategoriesWithEmptyInquiries_WhenAllRequested()
        {
            //arange
            var mock = new Mock<IRepository>();
            mock.Setup(z => z.Query<Inquiry>(x => x.Client, x => x.Tags)).Returns(new List<Inquiry>().AsQueryable());
            mock.Setup(z => z.Query<Tag>(x => x.Inquiries)).Returns(new List<Tag>
            {
                new Tag
                {
                    Inquiries = new List<Inquiry>(),
                    Name = "",
                    Id = 1
                }
            }.AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object, Substitute.For<IUrlHelper>());

            //act
            var viewResult = (AllInquiriesViewModel) inquiriesController.All().Model;

            //assert
            viewResult.Categories.Count().Should().Be(0);
        }

        [Test]
        public void Should_ReturnOnlyInquiriesWhereReferenceDateEqualNull_WhenAdminRequested()
        {
            // arrange
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>(z => z.Client, z => z.Assignee))
                .Returns(inquiries.AsQueryable());

            // act
            var controller = new InquiriesController(mock.Object, Substitute.For<IUrlHelper>());
            var viewResult = (AdminViewModel)controller.Admin().Model;

            // assert
            viewResult.Inquiries.Count().Should().Be(10);
        }
    }
}
