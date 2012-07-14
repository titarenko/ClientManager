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
            var end = today.GetEndOfBusinessWeek();

            var thisWeekInquiries = repository.Query<Inquiry>(x => x.Client)
                .Where(x => x.ReferenceDate >= start && x.ReferenceDate <= end)
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
                                Name = x.Client.FullName,
                                Subject = x.Subject,
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
    }
}