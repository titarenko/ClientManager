using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IRepository repository;

        public ClientsController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Returns photo of person
        /// </summary>
        /// <param name="personId">Id of person</param>
        /// <returns>Photo of person, if id of person is wrong return null</returns>
        public ActionResult ShowPhoto(int personId)
        {
            var currentClient = repository.Query<Person>().FirstOrDefault(x => x.Id == personId);
            return currentClient!=null ? File(currentClient.Photo, "image/jpg") : null;
        }

        public ViewResult Index()
        {
            return View(repository.Query<Person>().
                Where(client => client.Role == PersonRole.Client));
        }

        public ViewResult MailingHistory(int id)
        {
            var person = repository.Get<Person>(id, x => x.RelatedMails);
            person.RelatedMails = person.RelatedMails.OrderBy(x => x.Date).ToList();

            return View(person);

        }

        public ViewResult Details(int id)
        {
            return View(repository.Get<Person>(id));
        }

        public ViewResult Edit(int id)
        {
            return View(repository.Get<Person>(id));
        }

        [HttpPost]
        public ViewResult Edit(int id, Person client)
        {
            if (ModelState.IsValid)
            {
                repository.Save(client);
                return View("Details", client);
            }

            return View(client);
        }
    }
}