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

        public IList<Inquiry> SelectedDayInquiries(DateTime day, IRepository repository)
    {
        
        return repository.Query<Inquiry>().Where(inquiry => inquiry.ReferenceDate == day).ToList();
    }
        //
        // GET: /MonthView/

        public ActionResult Index()
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

            return View(model);        }

    }
}
