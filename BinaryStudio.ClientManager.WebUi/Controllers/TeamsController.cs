using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private IRepository repository;

        public TeamsController(IRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult Index()
        {
            return View();
        }
    }
}
