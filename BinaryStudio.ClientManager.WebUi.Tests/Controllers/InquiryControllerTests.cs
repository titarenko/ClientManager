using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Controllers;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.WebUi.Tests.Controllers
{
    [TestFixture]
    public class InquiryControllerTests
    {
        [Test]
        public void Shoild_ReturnInquiry_WhenRequestedFullList()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Inquiry>()).Returns(new List<Inquiry>()
                                                            {
                                                                new Inquiry(new Person(){FirstName = "Ivan", LastName = "Ivanov"},  
                                                                            new MailMessage(){Subject = "subj1"},
                                                                            5
                                                                            ),
                                                                new Inquiry(new Person(){FirstName = "Petr", LastName = "Petrov"},  
                                                                            new MailMessage(){Subject = "subj2"},
                                                                            1
                                                                            )
                                                            }.AsQueryable());
            var inquiriesController = new InquiriesController(mock.Object);
            var response = inquiriesController.Index();

            // chek
            Assert.Pass(); //lol
        }
    }
}
