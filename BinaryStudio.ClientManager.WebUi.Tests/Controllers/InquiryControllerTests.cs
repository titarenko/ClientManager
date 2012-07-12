using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Controllers;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using FizzWare.NBuilder.Generators;
using Moq;
using NSubstitute;
using NUnit.Framework;


namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class InquiryControllerTests
    {
        /// <summary>
        /// our test DB
        /// </summary>
        private static readonly List<Inquiry> ListInquiries = new List<Inquiry>() {
                                                 new Inquiry { 
                                                     Client = new Person
                                                         {
                                                             FirstName = "Ivan",
                                                             LastName = "Ivanov",
                                                             Role = (int)PersonRole.Client,
                                                             Id = 9
                                                         },
                                                     Source = new MailMessage
                                                                  {
                                                                      Subject = "subj1"
                                                                  },
                                                     Id = 5
                                                     },
                                                 new Inquiry {
                                                     Client = new Person
                                                         {
                                                             FirstName = "Petr",
                                                             LastName = "Petrov",
                                                             Role = (int)PersonRole.Client,
                                                             Id = 8
                                                         },
                                                     Source = new MailMessage
                                                                  {
                                                                      Subject = "subj2"
                                                                  },
                                                     Id = 1
                                                 }
                                             };

        [Test]
        public void Should_ReturnInquiry_WhenRequestedFullList()
        {
            // setup
            var mock = new Mock<IRepository>();
            mock.Setup(z => z.Query<Inquiry>(x => x.Client, x => x.Source)).Returns(ListInquiries.AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);

            // act
            var response = inquiriesController.Index();
            var typedResponse = response as ViewResult;
            var list = typedResponse.Model as IEnumerable<Inquiry>;

            // chek
            Assert.That(list.Single(x => x.Client.LastName == "Petrov") != null &&
                list.Single(x => x.Source.Subject == "subj2") != null);
        }

        [Test]
        public void ShouldNot_RaiseException_WhenDbIsEmpty()
        {
            // setup
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>()).Returns(new List<Inquiry>().AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);

            // act
            var response = inquiriesController.Index() as ViewResult;

            // check
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
            var inquiriesController = new InquiriesController(mock.Object);

            // act
            var viewResult = inquiriesController.WeekView().Model as IEnumerable<Inquiry>;

            // assert
            Assert.That(viewResult.Count() == 5);

            using (var inquiry = viewResult.GetEnumerator())
            {
                inquiry.MoveNext();
                var prev = inquiry.Current;
                while (inquiry.MoveNext())
                {
                    Assert.That(prev.ReferenceDate <= inquiry.Current.ReferenceDate);
                    prev = inquiry.Current;
                }
            }
        }

        [Test]
        public void ShouldNot_RaiseAnException_WhenWeekViewRequestedAndRepositoryIsEmpty()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>(z => z.Client, z => z.Source, z => z.Assignee))
                .Returns(new List<Inquiry>().AsQueryable());

            var inquiriesController = new InquiriesController(mock.Object);

            Assert.DoesNotThrow(() => inquiriesController.WeekView());
        }
    }
}
