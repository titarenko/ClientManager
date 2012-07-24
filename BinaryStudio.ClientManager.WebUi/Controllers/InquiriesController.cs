using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
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
            return View(repository.Get<Inquiry>(id, x => x.Client, x => x.Source,
                x => x.Source.Sender, x => x.Comments, x => x.Assignee));
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
            return View(new AllInquiriesViewModel
            {
                Categories = repository.Query<Inquiry>(x => x.Tags)
                    .GroupBy(x => x.Tags.FirstOrDefault().Name)
                    .Select(all => new CategoryViewModel()
                                        {
                                            Tag =
                                                all.Select(inquiry => inquiry.Tags.FirstOrDefault()).
                                                FirstOrDefault(),
                                            Inquiries =
                                                all.Select(
                                                    inquiry => new TaggedInquiryViewModel
                                                                    {
                                                                        Id = inquiry.Id,
                                                                        FirstName = inquiry.Client.FirstName,
                                                                        LastName = inquiry.Client.LastName,
                                                                        Subject = inquiry.Subject,
                                                                    })
                                        }
                    ).AsEnumerable()
            });
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

        public ViewResult Month() 
        {
            var today = Clock.Now;
            var start = today.GetStartOfMonth();
            var end = today.GetEndOfMonth().AddDays(1);
            var skipDaysCount = 0;
            var startWeek = start.WeekNumber();
            if (start.DayOfWeek == DayOfWeek.Saturday || 
                start.DayOfWeek == DayOfWeek.Sunday)
            {
                startWeek++;
            }
            var finishWeek = end.AddDays(-1).WeekNumber();
            switch (start.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    skipDaysCount = 1;
                    break;
                case DayOfWeek.Wednesday:
                    skipDaysCount = 2;
                    break;
                case DayOfWeek.Thursday:                    
                    skipDaysCount = 3;
                    break;
                case DayOfWeek.Friday:
                    skipDaysCount = 4;
                    break;
                default:
                    skipDaysCount = 0;
                    break;
            }

            var inquiryThisMonthList = repository.Query<Inquiry>(x => x.Client).
                Where(x => x.ReferenceDate >= start && x.ReferenceDate < end).ToList();
            
            return View(new MonthViewModel
            {
                SkippedDays = skipDaysCount,
                StartingWeek = startWeek,
                FinishingWeek = finishWeek,
                Days = (
                    from index in Enumerable.Range(0, (end - start).Days)
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
                            }).AsEnumerable()
                    }).AsEnumerable(),
            });
        }

        public ViewResult Admin()
        {
            return View(new AllInquiriesViewModel
            {
                Categories = repository.Query<Inquiry>(x => x.Tags)
                    .Where(x => x.Status == InquiryStatus.IncomingInquiry)
                    .GroupBy(x => x.Tags.FirstOrDefault().Name)
                    .Select(all => new CategoryViewModel
                    {
                        Tag = all.Select(inquiry => inquiry.Tags.FirstOrDefault())
                            .FirstOrDefault(),
                        Inquiries = all.Select(
                            inquiry => new TaggedInquiryViewModel
                            {
                                Id = inquiry.Id,
                                FirstName = inquiry.Client.FirstName,
                                LastName = inquiry.Client.LastName,
                                Subject = inquiry.Subject,
                            })
                    }).ToList()
            });
        }
    }
}