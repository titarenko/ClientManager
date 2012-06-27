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

        public ViewResult Index()
        {
            return View(repository.Query<Person>().
                Where(client => client.Role == PersonRole.Client));
        }

        /// <summary>
        /// Test method to add Person
        /// MUST BE DELETED!!!
        /// </summary>
        /// <returns></returns>
        public ViewResult Create()
        {
            Person person = new Person();
            return View(person);
        }

        [HttpPost]
        public ViewResult Create(Person person)
        {
            if(ModelState.IsValid)
            {
                repository.Save(person);
            }
            return View("Index");
        }

        public ViewResult MailingHistory(int id)
        {
            return View(repository.Query<MailMessage>().Where(message => message.Sender.Id == id));
        }
    }
}