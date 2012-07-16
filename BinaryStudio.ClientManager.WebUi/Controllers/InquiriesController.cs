using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Models;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class InquiriesController : Controller
    {
        private readonly IRepository repository;

        public InquiriesController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// GET: /Inquiries/
        /// </summary>
        public ViewResult Index()
        {
            return View(repository.Query<Inquiry>(x => x.Client, x => x.Source).OrderBy(x => x.Client.Id));
        }

        /// <summary>
        /// GET: /Inquiries/5
        /// </summary>
        public ViewResult Details(int id)
        {
            return View(repository.Get<Inquiry>(id, x => x.Client, x => x.Source, x => x.Source.Sender));
        }

        public ViewResult Edit(int id)
        {
            return View(repository.Get<Inquiry>(id, x => x.Client, x => x.Source, x => x.Source.Sender));
        }

        [HttpPost]
        public ViewResult Edit(int id, Inquiry inquiry)
        {
            if(ModelState.IsValid)
            {
                repository.Save(inquiry);
                repository.Save(inquiry.Source);
                repository.Save(inquiry.Client);
                repository.Save(inquiry.Source.Sender);
                return View("Details", inquiry);
            }

            return View(inquiry);

        }

        public ActionResult MailMessage(int id)
        {
            return View(repository.Get<MailMessage>(id, x => x.Sender, x => x.Receivers));
        }

        public IList<Inquiry> SelectedDayInquiries(DateTime day, IRepository repository)
        {

            return repository.Query<Inquiry>().Where(inquiry => inquiry.ReferenceDate == day).ToList();
        }
        //
        // GET: /MonthView/

        public ActionResult MonthView()
        {
            ViewBag.currentDay = DateTime.Today;
            var model = new MonthViewModel();
            var inquiryFutureList = repository.Query<Inquiry>().
                Where(inquiry => inquiry.ReferenceDate >= DateTime.Today).ToList();

            model.Inquiries = inquiryFutureList;

            var MonthList = SelectedDayInquiries(DateTime.Today, repository);
            for (int i = 1; i <= 31; i++)
            {
                var day = DateTime.Today.AddDays(i);
                var dayInquiryList = SelectedDayInquiries(day, repository);
                MonthList.Concat(dayInquiryList);
            }

            model.MonthViewItems = MonthList;

            //var monthviewItems = new MonthViewItem();
            //inquiryFutureList.ForEach(monthviewItems.ReferenceDate = inquiryFutureList[this].ReferenceDate, 
            //    monthviewItems.Client = inquiryFutureList[this].Client);

            //model.MonthViewItems = monthviewItems;

            return View(model);
        }

    }
}
