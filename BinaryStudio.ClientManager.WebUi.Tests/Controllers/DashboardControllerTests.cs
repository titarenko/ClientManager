using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Controllers;
using BinaryStudio.ClientManager.WebUi.Models;
using FizzWare.NBuilder;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class DashboardControllerTests
    {
        /// <summary>
        /// Test List of inquires
        /// </summary>
        private static List<Inquiry> ListInquiries()
        { 
            return new List<Inquiry>
                       {
                           new Inquiry
                               {
                                   Client = new Person(),
                                   Source = new MailMessage
                                                {
                                                    Sender = new Person(),
                                                    Receivers = new Collection<Person>()
                                                }
                               },
                           new Inquiry
                               {
                                   Client = new Person(),
                                   Source = new MailMessage
                                                {
                                                    Sender = new Person(),
                                                    Receivers = new Collection<Person>()
                                                }
                               }
                       };
        }

        /// <summary>
        /// Test list of persons
        /// </summary>
        private static IEnumerable<Person> ListPersons()
        {
            return new List<Person>
                       {
                           new Person
                               {
                                   FirstName = "Client",
                                   Role = PersonRole.Client,
                                   Id = 1
                                   
                               },
                           new Person
                               {
                                   FirstName = "Employee1",
                                   Role = PersonRole.Employee,
                                   Id = 2
                               },
                           new Person
                               {
                                   FirstName = "Employee2",
                                   Role = PersonRole.Employee,
                                   Id = 3
                               }
                       };
        }

        [Test]
        public void Should_ReturnListOfAllInquiriesAndListOfPersonsWithEmployeeRole_WhenRequestedIndexMethod()
        {
            //arrange
            var mock = new Mock<IRepository>();
            mock.Setup(z => z.Query<Inquiry>(x => x.Client, x => x.Source)).Returns(ListInquiries().AsQueryable());
            mock.Setup(x => x.Query<Person>()).Returns(ListPersons().AsQueryable());
            var dashboardController = new DashboardController(mock.Object);
            
            var expectedPersons = new List<Person>
                                      {
                                          new Person
                                              {
                                                  FirstName = "Employee1",
                                                  Role = PersonRole.Employee,
                                                  Id = 2
                                              },
                                          new Person
                                              {
                                                  FirstName = "Employee2",
                                                  Role = PersonRole.Employee,
                                                  Id = 3
                                              }
                                      }.ToList();

            //act
            var returnedView = dashboardController.Index() as ViewResult;

            //converts View.Model to DashboardModel
            var returnedModel = returnedView.Model as DashboardModel;

            //assert
            Assert.AreEqual(2, returnedModel.Inquiries.Count(), "count");
            CollectionAssert.Contains(returnedModel.Inquiries,ListInquiries()[0], "first element");
            CollectionAssert.Contains(returnedModel.Inquiries, ListInquiries()[1], "second element");

            Assert.AreEqual(2, returnedModel.Employees.Count(), "Employee's count");
            CollectionAssert.Contains(returnedModel.Employees, expectedPersons[0], "first element");
            CollectionAssert.Contains(returnedModel.Employees, expectedPersons[1], "second element");

        }

        [Test]
        public void ShouldNot_RaiseException_WhenRepositoryIsEmpty()
        {
            //arrange
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>()).Returns(new List<Inquiry>().AsQueryable());
            mock.Setup(x => x.Query<Person>()).Returns(new List<Person>().AsQueryable());
            var dashboardController = new DashboardController(mock.Object);

            //act
            var result = dashboardController.Index();

            //assert
            Assert.IsNotNull(result);
        }
    }
}
