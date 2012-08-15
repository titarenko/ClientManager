using System;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.WebUi.Models;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    [Authorize]
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
                x => x.Source.Sender, x => x.Comments, x => x.Assignee, x => x.Tags));
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

            var thisWeekInquiries = repository.Query<Inquiry>(x => x.Client,x=>x.Owner)
                .Where(x => x.ReferenceDate >= start && x.ReferenceDate < end)
                .ToList();

            return View(new WeekViewModel
            {
                Days =
                    from index in Enumerable.Range(0, 5)
                    let date = start.AddDays(index)
                    select new WeekItemViewModel
                    {
                        Name = "{0} ({1})".Fill(date.ToString("ddd"), date.ToString("dd.MM")),
                        Date = date,
                        Inquiries = thisWeekInquiries
                            .Where(x => x.ReferenceDate.Value.Date == date)
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
                    .ToList(),

                Tags = repository.Query<Tag>().ToList()
            });
        }

        public ViewResult Month()
        {
            var today = Clock.Now.Date;
            var begin = today.GetStartOfMonth();
            var end = today.GetEndOfMonth().AddDays(1);

            var monthInquiries = repository
                .Query<Inquiry>(x => x.Client)
                .Where(x => x.ReferenceDate >= begin && x.ReferenceDate < end)
                .OrderBy(x => x.ReferenceDate)
                .ToList();

            monthInquiries.RemoveAll(x => x.ReferenceDate.Value.IsWeekend());

            var start = begin.IsWeekend() ?
                begin.AddDays(7).GetStartOfBusinessWeek() :
                begin.GetStartOfBusinessWeek();

            var firstWeek = start.GetWeekNumber();
            var lastWeek = end.GetWeekNumber();

            return View("Month", new MonthViewModel
            {
                Name = today.ToString("MMMM"),
                MaxInquiriesWithoutToggling = 3,
                Weeks =
                    from week in Enumerable.Range(0, lastWeek - firstWeek + 1)
                    let weekStart = start.AddDays(week * 7)
                    select new MonthItemViewModel
                    {
                        Days = from day in Enumerable.Range(0, 5)
                               let date = weekStart.AddDays(day)
                               select new WeekItemViewModel
                               {
                                   Name = date.ToString("ddd"),
                                   Date = date,
                                   Inquiries = monthInquiries
                                        .Where(x => x.ReferenceDate.Value.Date == date)
                                        .OrderBy(x => x.ReferenceDate)
                                        .Select(x => new InquiryViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Client.FullName,
                                            Subject = x.Subject,
                                        })
                                }

                    }
            });
        }

        /// <summary>
        /// shows all inquiries
        /// </summary>
        /// <returns></returns>
        public ViewResult All()
        {
            var categoryWithEmptyTag =
                new CategoryViewModel
                {
                    Tag = new TagViewModel { Name = "" },
                    Inquiries = repository
                        .Query<Inquiry>()
                        .Where(x => !x.Tags.Any() && x.ReferenceDate.HasValue)
                        .Select(inquiry => new TaggedInquiryViewModel
                        {
                            Id = inquiry.Id,
                            FirstName = inquiry.Client.FirstName,
                            LastName = inquiry.Client.LastName,
                            Subject = inquiry.Subject
                        })
                        .ToList()
                };

            var categories = repository
                .Query<Tag>()
                .Where(x => x.Inquiries.Any(z => z.ReferenceDate.HasValue))
                .Select(tag => new CategoryViewModel
                {
                    Tag = new TagViewModel
                    {
                        Name = tag.Name,
                    },
                    Inquiries = tag.Inquiries
                        .Where(x => x.ReferenceDate.HasValue)
                        .Select(x => new TaggedInquiryViewModel
                        {
                            Id = x.Id,
                            FirstName = x.Client.FirstName,
                            LastName = x.Client.LastName,
                            Subject = x.Subject
                        })
                })
                .ToList();

            if (categoryWithEmptyTag.Inquiries.Any())
            {
                categories.Add(categoryWithEmptyTag);
            }

            return View(new AllInquiriesViewModel
            {
                InquiryDetailsUrl = Url.Action("Details", "Inquiries"),
                Categories = categories
            });
        }

        public ViewResult Admin()
        {
            var inquiries = repository
                .Query<Inquiry>(x => x.Client, x => x.Assignee)
                .Where(x => x.ReferenceDate == null)
                .OrderBy(x => x.Client.FirstName)
                .ThenBy(x => x.Client.LastName)
                .ToList();

            return View(
                new AdminViewModel
                    {
                        Inquiries = inquiries
                            .Select(x => new InquiryViewModel
                            {
                                Id = x.Id,
                                Name = x.Client.FullName,
                                Subject = x.Subject,
                                Email = x.Client.Email,
                                Assignee = x.SafeGet(z => z.Assignee.FullName),
                                Phone = x.Client.Phone,
                                PhotoUri = x.Client.PhotoUri
                            }),

                        Employees = repository.Query<Person>()
                            .Where(x => x.Role == PersonRole.Employee)
                            .OrderBy(x => x.FirstName)
                            .ThenBy(x => x.LastName)
                            .ToList(),

                        Tags = repository.Query<Tag>()
                            .OrderBy(x => x.Name)
                            .ToList()
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
            if (inquiry == null || date.IsWeekend())
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

        [HttpPost]
        public void AddTag(int inquiryId, int tagId)
        {
            var inquiry = repository.Get<Inquiry>(inquiryId);
            var tag = repository.Get<Tag>(tagId);
            if (inquiry == null || tag == null)
            {
                throw new ModelIsNotValidException();
            }
            inquiry.Tags.Add(tag);
            repository.Save(inquiry);
        }
    }
}