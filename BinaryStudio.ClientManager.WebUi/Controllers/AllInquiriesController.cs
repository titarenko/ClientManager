using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Models;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class AllInquiriesController : Controller
    {
        private readonly IRepository repository;

        public AllInquiriesController(IRepository repository)
        {
            this.repository = repository;
        }

        //
        // GET: /AllInquiries/
        public ActionResult Index()
        {
            var model = new AllInquiriesModel();
            model.Inquiries = repository.Query<Inquiry>(x => x.Client, x => x.Subject, x => x.Taggs).ToList();
            model.Tags = repository.Query<Tag>(x => x.Id, x => x.Name).ToList();
            return View(model);
        }
    }
}
