using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using System.Linq;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IRepository repository;

        public ClientsController(IRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult Clients()
        {
            return View(repository.Query<Person>().
                Where(client => client.Role == PersonRole.Client));
        }
    }
}