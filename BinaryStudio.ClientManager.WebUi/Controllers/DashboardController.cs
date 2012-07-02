using System;
using System.Collections.Generic;
using System.Globalization;
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
            var employeesFullList = repository.Query<Person>().
                 Where(person => person.RoleValue == (int)PersonRole.Employee);

            model.Inquiries = repository.Query<Inquiry>(x => x.Client, x => x.Source);

            var employeesModelList = employeesFullList.Select(employee => new SelectListItem
                                                                              {
                                                                                  Value = employee.Id.ToString(CultureInfo.InvariantCulture), Text = employee.FullName
                                                                              }).ToList();
            model.Employees = employeesModelList;

            return View(model);
        }

    }
}
