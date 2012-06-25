using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class InquiriesController : Controller
    {
        private readonly IRepository repository;
        private InquiriesController()
        {
            throw new InvalidOperationException();
        }

        public InquiriesController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// GET: /Inquiries/
        /// </summary>
        public ActionResult Index()
        {
            return View(repository.Query<Inquiry>().OrderBy(x => x.Issuer.Id));//(i => i.Issuer.Role == PersonRole.Client).OrderBy(x => x.Issuer.Id));
        }

        /// <summary>
        /// GET: /Inquiries/5
        /// </summary>
        public ActionResult Index(int id)
        {
            return View(new List<Inquiry>() { repository.Get<Inquiry>(id) } );
        }
        /*
        //
        // GET: /Inquiries/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Inquiries/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Inquiries/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Inquiries/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Inquiries/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Inquiries/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/
    }
}
