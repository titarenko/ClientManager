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


namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class InquiryControllerTests
    {
        private IList<Inquiry> inquiries;
        
        [SetUp]
        public void GenerateInquiriesList()
        {
            inquiries = Builder<Inquiry>.CreateListOfSize(30)
                .All()
                .With(x => x.Client = Builder<Person>.CreateNew().Build())
                .With(x => x.Assignee = Builder<Person>.CreateNew().Build())
                .With(x => x.Source = Builder<MailMessage>.CreateNew().Build())
                .TheFirst(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(January.The1st, January.The10th))
                .TheNext(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(February.The1st, February.The10th))
                .TheNext(10)
                .With(x => x.ReferenceDate = GetRandom.DateTime(March.The1st, March.The10th))
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
            Assert.That(list.Count() == 30);
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
            var mock = new Mock<IRepository>();
            mock.Setup(z => z.Query<Inquiry>(x => x.Client, x => x.Source)).Returns(inquiries.AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);

            var viewModel = inquiriesController.Week();
            var inquiriesList = (viewModel.Model as WeekViewModel).Days;

            var inquiriesCount = 0;
            foreach (var day in inquiriesList)
            {
                inquiriesCount += day.Inquiries.Count();
            }


        }

        [Test]
        public void Should_ReturnListOfInquiriesForCurrentMonth_WhenRequested()
        {
        }
    }
}
