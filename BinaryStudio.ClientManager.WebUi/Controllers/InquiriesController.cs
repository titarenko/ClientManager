using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

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
        public ActionResult Index()
        {
            return View(repository.Query<Inquiry>(x => x.Client, x => x.Source).OrderBy(x => x.Client.Id));
        }

        /// <summary>
        /// GET: /Inquiries/5
        /// </summary>
        public ActionResult Details(int id)
        {
            return View(repository.Get<Inquiry>(id, x => x.Client, x => x.Source, x => x.Source.Sender));
        }

        public ActionResult Edit(int id)
        {
            return View(repository.Get<Inquiry>(id, x => x.Client, x => x.Source));
        }

        [HttpPost]
        public ActionResult Edit(int id, Inquiry inquiry)
        {
            if(ModelState.IsValid)
            {
                return View(inquiry);
            }

            repository.Save(inquiry);
            return View("Details", inquiry);
        }
    }
}
