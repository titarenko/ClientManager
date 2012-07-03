using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Models;
using FizzWare.NBuilder;

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

            /* model.Employees = new SelectList(
                repository.Query<Person>()
                    .Where(person => person.RoleValue == (int) PersonRole.Employee)
                   .Select(employee => new SelectListItem
                                            {
                                                Value = employee.Id.ToString(),
                                                Text = employee.FullName
                                            })
                    .ToList());
            

            var allInquiries = repository.Query<Inquiry>(x => x.Client, x => x.Source);

            model.Inquiries = allInquiries.Where(i => i.Status == InquiryStatus.IncomingInquiry).ToList();

            model.WaitingForReply = allInquiries.Where(i => i.Status == InquiryStatus.WaitingForReply).ToList();

            model.InProgress = allInquiries.Where(i => i.Status == InquiryStatus.InProgress).ToList();
             */

            Func<int, IList<Inquiry>> q =
                x =>
                Builder<Inquiry>.CreateListOfSize(x).All().With(
                    i => i.Source = new MailMessage { Subject = "Param pam pam" }).Build();
            model.Inquiries = q(15);
            model.WaitingForReply = q(4);
            model.InProgress = q(5);
            return View(model);
        }

    }
}
