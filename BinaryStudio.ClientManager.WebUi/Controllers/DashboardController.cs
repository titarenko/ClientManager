using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Models;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        private readonly IRepository repository;

        public DashboardController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            DashboardModel model = new DashboardModel();
            var employeesModelList = new List<SelectListItem>();
            var employeesFullList = repository.Query<Person>().
                 Where(person => person.RoleValue == (int)PersonRole.Employee);

            model.Inquiries = repository.Query<Inquiry>(x => x.Client, x => x.Source);

            foreach (var employee in employeesFullList)
            {
                var id = employee.Id.ToString();
                employeesModelList.Add(new SelectListItem
                                           {
                                               Value = id,
                                               Text = employee.FullName
                                           });
            }

            model.Employees = employeesModelList;

            return View(model);
        }

    }
}
