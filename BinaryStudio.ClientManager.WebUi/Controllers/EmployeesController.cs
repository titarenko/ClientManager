using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class EmployeesController : Controller
    {
        private IRepository repository;

        public EmployeesController(IRepository repository)
        {
            this.repository = repository;
        }

        //
        // GET: /Employees/
        public ViewResult Index()
        {
            return View(repository.Query<Person>().ToList().Where(x=>x.Role==PersonRole.Employee));
        }

    }
}
