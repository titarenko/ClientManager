using System;
using System.Collections.Generic;
using System.Linq;
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
            var model = new DashboardModel();
            var employeesFullList = repository.Query<Person>().
                Where(person => person.Role == PersonRole.Employee).ToList();


            model.Employees = employeesFullList;


            var allInquiries = repository.Query<Inquiry>(x => x.Client, x => x.Source);

            model.Inquiries = allInquiries.Where(i => i.Status == InquiryStatus.IncomingInquiry).ToList();

            model.WaitingForReply = allInquiries.Where(i => i.Status == InquiryStatus.WaitingForReply).ToList();

            model.InProgress = allInquiries.Where(i => i.Status == InquiryStatus.InProgress).ToList();

            return View(model);
        }

    }
}
