using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        private readonly IRepository repository;

        public TestController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Test method to clear the persons
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearPersons()
        {
            IList<Person> personList = repository.Query<Person>().ToList();
            foreach (var person in personList)
            {
                repository.Delete(person);
                
            }
            return RedirectToAction("index", "Clients");
        }

        /// <summary>
        /// Test method to add Person
        /// </summary>
        /// <returns></returns>
        public ActionResult CreatePersons()
        {
            var random = new Random(DateTime.Now.Second);
            var persons = new Person[10];
            for (int i = 0; i < 10; i++)
            {
                int randomInt = random.Next(1, 100);
                persons[i] = new Person
                {
                    FirstName = "Person" + randomInt,
                    LastName = "Surname" + randomInt,
                    Email = randomInt + "@mail.ru",
                    CreationDate = DateTime.Now,
                    Role = random.Next(0, 2)
                };
                repository.Save(persons[i]);
            }
            return RedirectToAction("Index", "Clients");
        }

        public ActionResult CreateInquiries()
        {
            Random random = new Random(DateTime.Now.Second);
            Inquiry [] inquiries = new Inquiry[10];
            List<Person> clients = repository.Query<Person>().
                Where(x => x.Role == (int)PersonRole.Client).ToList();
            
            for (int i = 0; i < 10; i++)
            {
                int randomInt = random.Next(1, 100);
                inquiries[i] = new Inquiry
                                   {
                                       Client = clients[random.Next(clients.Count)],
                                       Source = new MailMessage
                                                    {
                                                        Body = "MailMessage " + randomInt + " Text",
                                                        Date = DateTime.Now,
                                                        Receivers = new Collection<Person>(),
                                                        Subject = "Subject" + randomInt,
                                                        Sender = new Person
                                                                     {
                                                                         CreationDate = DateTime.Now
                                                                     }
                                                    }
                                   };
                repository.Save(inquiries[i]);
            }
            return RedirectToAction("Index", "Inquiries");
        }
    }
}
