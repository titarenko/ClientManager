﻿using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Models;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly IRepository repository;

        public ClientsController(IRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult Index()
        {
            return View(repository.Query<Person>().
                Where(client => client.Role == PersonRole.Client));
        }

        public ViewResult MailingHistory(int id)
        {
            return View(repository.Query<MailMessage>(x => x.Sender, x => x.Receivers)
                .Where(x => x.Sender.Id == id || x.Receivers.Any(y => y.Id == id))
                .OrderBy(x => x.Date)
                .ToList());
        }

        public ViewResult Details(int id)
        {
            return View(new ClientDetailsViewModel
            {
                Client = repository.Get<Person>(id),
                Inquiries = repository.Query<Inquiry>().Where(x => x.Client.Id == id)
            });
        }

        public ViewResult Edit(int id)
        {
            return View(repository.Get<Person>(id));
        }

        [HttpPost]
        public ActionResult AddPhoto(int id, HttpPostedFileBase photo)
        {
            if (photo != null && photo.ContentLength>0)
            {
                var pathToPhoto = Path.Combine(Server.MapPath("~/Content/photos"), id.ToString() + Path.GetExtension(photo.FileName));
                photo.SaveAs(pathToPhoto);
                var client = repository.Get<Person>(id);
                client.PhotoUri = "~/Content/photos/" + Path.GetFileName(pathToPhoto);
                repository.Save(client);
            }
            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        public ActionResult Edit(int id, Person client)
        {
            if (ModelState.IsValid)
            {
                repository.Save(client);
                return RedirectToAction("Details", new {id});
            }

            return View(client);
        }
    }
}