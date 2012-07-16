using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IRepository repository;

        public EmployeesController(IRepository repository)
        {
            this.repository = repository;
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
                return View("Details", employee);
            }

            return View(employee);
        }
    }
}
