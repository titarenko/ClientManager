using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.WebUi.Models;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class MonthViewController : Controller
    {
        private readonly IRepository repository;

        public MonthViewController(IRepository repository)
        {
            this.repository = repository;
        }
        //
        // GET: /MonthView/

        public ActionResult Index()
        {
            ViewBag.currentDay = DateTime.Today;
            var model = new MonthViewModel();
            var inquiryTodayList = repository.Query<Inquiry>().
                Where(inquiry => inquiry.ReferenceDate == DateTime.Today).ToList();

            model.Inquiries = inquiryTodayList; // Fill inquiries with today inquiries

            return View();
        }

    }
}
