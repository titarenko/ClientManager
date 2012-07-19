using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
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
            if (ModelState.IsValid)
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

        public ViewResult Week()
        {
            var today = Clock.Now.Date;

            var start = today.GetStartOfBusinessWeek();
            var end = today.GetEndOfBusinessWeek().AddDays(1);

            var thisWeekInquiries = repository.Query<Inquiry>(x => x.Client)
                .Where(x => x.ReferenceDate >= start && x.ReferenceDate < end)
                .ToList();

            return View(new WeekViewModel
            {
                Days =
                    from index in Enumerable.Range(0, 5)
                    let date = start.AddDays(index)
                    select new WeekItemViewModel
                    {
                        Name = date.ToString("ddd"),
                        Date = date,
                        Inquiries = thisWeekInquiries
                            .Where(x => x.ReferenceDate.Date == date)
                            .OrderBy(x => x.ReferenceDate)
                            .Select(x => new InquiryViewModel
                            {
                                Id = x.Id,
                                Name = x.Client.FullName,
                                Subject = x.Subject,
                                Email = x.Client.Email,
                                Assignee = x.SafeGet(z => z.Assignee.FullName),
                                Phone = x.Client.Phone,
                                PhotoUri = x.Client.PhotoUri
                            })
                    },
                Employees = repository.Query<Person>()
                    .Where(x => x.Role == PersonRole.Employee)
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList()
            });
        }

        /// <summary>
        /// shows all inquiries
        /// </summary>
        /// <returns></returns>
        public ViewResult All()
        {
            return View(repository.Query<Inquiry>()
                .GroupBy(x => x.Tags.FirstOrDefault().Name)
                .Select(all => new AllItemModel
                    {
                        Tag = all.Key,
                        Inquiries = all.Select(inquiry => new AllInquiryViewModel
                                                              {
                                                                  FirstName = inquiry.Client.FirstName, LastName = inquiry.Client.LastName, Subject = inquiry.Subject
                                                              })
                    })
                .ToList());
        }

        [HttpPost]
        public void AssignTo(int inquiryId, int employeeId)
        {
            var inquiry = repository.Get<Inquiry>(inquiryId);
            var person = repository.Get<Person>(employeeId);
            if (inquiry == null || person == null)
            {
                throw new ModelIsNotValidException();
            }
            inquiry.Assignee = person;
            repository.Save(inquiry);
        }

        [HttpPost]
        public void MoveTo(int inquiryId, DateTime date)
        {
            var inquiry = repository.Get<Inquiry>(inquiryId);
            if (inquiry == null)
            {
                throw new ModelIsNotValidException();
            }
            inquiry.ReferenceDate = date;
            repository.Save(inquiry);
        }

        [HttpPost]
        public void AddComment(int inquiryId, string text)
        {
            var inquiry = repository.Get<Inquiry>(inquiryId);
            if (inquiry == null)
            {
                throw new ModelIsNotValidException();
            }
            inquiry.Comments.Add(new Comment
            {
                Date = Clock.Now,
                Text = text
            });
            repository.Save(inquiry);
        }
        
        public IList<Inquiry> SelectedDayInquiries(DateTime day, IRepository repository)
        {

            return repository.Query<Inquiry>().Where(inquiry => inquiry.ReferenceDate == day).ToList();
        }
        //
        // GET: /MonthView/

        public ActionResult Month()
        {
            var today = DateTime.Today;
            var currentMonth = DateTime.Today.Month;
            var model = new MonthViewModel();
            var inquiryThisMonthList = repository.Query<Inquiry>().
                Where(inquiry => inquiry.ReferenceDate.Month == currentMonth).ToList();
            
            //model.Inquiries = inquiryFutureList;

            //var MonthList = SelectedDayInquiries(DateTime.Today, repository);
            //for (int i = 1; i <= 31; i++)
            //{
            //    var day = DateTime.Today.AddDays(i);
            //    var dayInquiryList = SelectedDayInquiries(day, repository);
            //    MonthList.Concat(dayInquiryList);
            //}

            //model.MonthViewItems = MonthList;

            //var monthviewItems = new MonthViewItem();
            //inquiryFutureList.ForEach(monthviewItems.ReferenceDate = inquiryFutureList[this].ReferenceDate, 
            //    monthviewItems.Client = inquiryFutureList[this].Client);

            //model.MonthViewItems = monthviewItems;
            var start = today.GetStartOfMonth();
            var end = today.GetEndOfMonth().AddDays(1);
            
            return View(new MonthViewModel
            {
                Days =
                    from index in Enumerable.Range(0, 30)
                    let date = start.AddDays(index)
                    select new MonthItemViewModel
                    {
                        Name = date.ToString("ddd"),
                        Date = date,
                        Inquiries = inquiryThisMonthList
                            .Where(x => x.ReferenceDate.Date == date)
                            .OrderBy(x => x.ReferenceDate)
                            .Select(x => new InquiryViewModel
                            {
                                Id = x.Id,
                                Name = x.Client.FullName,
                                Subject = x.Subject,
                                Email = x.Client.Email,
                                Assignee = x.SafeGet(z => z.Assignee.FullName),
                                Phone = x.Client.Phone,
                                PhotoUri = x.Client.PhotoUri
                            })
                    },
                /*Employees = repository.Query<Person>()
                    .Where(x => x.Role == PersonRole.Employee)
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList()*/
            });
        }
    }
}