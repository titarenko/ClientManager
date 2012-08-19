using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IRepository repository;
        private IAppContext appContext;

        public EmployeesController(IRepository repository, IAppContext appContext)
        {
            this.repository = repository;
            this.appContext = appContext;
        }

        //
        // GET: /Employees/
        public ViewResult Index()
        {
            return View(repository.Query<Person>().ToList().Where(x=>x.Role==PersonRole.Employee).AsQueryable());
        }

        public ViewResult Details(int id)
        {
            return View(repository.Get<Person>(id));
        }

        public ViewResult Edit(int id)
        {
            return View(repository.Get <Person>(id));
        }

        [HttpPost]
        public ViewResult Edit(int id, Person employee)
        {
            if(ModelState.IsValid)
            {
                repository.Save(employee);
                appContext.CurrentUser.RelatedPerson = employee;
                return View("Details", employee);
            }

            return View(employee);
        }
    }
}
