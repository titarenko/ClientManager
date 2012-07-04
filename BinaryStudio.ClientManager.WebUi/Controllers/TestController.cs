using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using FizzWare.NBuilder.Generators;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class RandomItem<T>
    {
        private readonly IList<T> list;
        private readonly IRandomGenerator randomGenerator;

        public RandomItem(IList<T> list, bool random )
        {
            this.list = list;
            this.randomGenerator = random ? new RandomGenerator() : new UniqueRandomGenerator();
        }

        public T Next()
        {
            return list[randomGenerator.Next(0, list.Count - 1)];
        }
    }
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
            var persons = Builder<Person>.CreateListOfSize(10)
                .All()
                .With(x => x.FirstName = GetRandom.FirstName())
                .With(x => x.LastName = GetRandom.LastName())
                .With(x => x.Role = GetRandom.Int(0, 2))
                .With(x => x.Country = GetRandom.String(10))
                .With(x => x.Email = GetRandom.Email())
                .With(x => x.CreationDate = GetRandom.DateTime(January.The1st, DateTime.Now))
                .With(x => x.Id = 0)
                .With(x => x.RelatedMails = Builder<MailMessage>.CreateListOfSize(5)
                    .All()
                    .With(z => z.Date = GetRandom.DateTime(January.The1st, DateTime.Now))
                    .With(z => z.Subject = GetRandom.String(10))
                    .With(z => z.Body = GetRandom.String(50))
                    .With(z => z.Id = 0)
                    .Build())
                .Build();

            foreach (var person in persons)
            {
                repository.Save(person);
            }
            return RedirectToAction("Index", "Clients");
        }

        public ActionResult CreateInquiries()
        {
            var random = new Random(DateTime.Now.Second);
            
            var clients = repository.Query<Person>(x => x.RelatedMails).
                Where(x => x.Role == PersonRole.Client).ToList();

            var randomClient = new RandomItem<Person>(clients, true);

            var iquiries = Builder<Inquiry>.CreateListOfSize(10)
                .All()
                .With(x => x.Status = GetRandom.Int(0, 3))
                .With(x => x.Client = randomClient.Next())
                .With(x => x.Id = 0)
                .With(x => x.Source = Builder<MailMessage>.CreateNew()
                                          .With(z => z.Date = GetRandom.DateTime(January.The1st, DateTime.Now))
                                          .With(z => z.Subject = GetRandom.String(10))
                                          .With(z => z.Body = GetRandom.String(50))
                                          .With(z => z.Id = 0)
                                          .Build())
                .Build();
            foreach (var inquiry in iquiries)
            {
                repository.Save(inquiry);
            }
            return RedirectToAction("Index", "Inquiries");
        }
    }
}
