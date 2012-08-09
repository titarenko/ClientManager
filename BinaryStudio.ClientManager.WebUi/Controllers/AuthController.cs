using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.WebUi.Models;
using OAuth2.Client;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepository repository;
        private readonly GoogleClient googleClient;
        private readonly FacebookClient facebookClient;

        public AuthController(IRepository repository, GoogleClient googleClient, FacebookClient facebookClient)
        {
            this.repository = repository;
            this.googleClient = googleClient;
            this.facebookClient = facebookClient;
        }

        /// <summary>
        /// Renders page with login links
        /// </summary>
        public ActionResult LogOn()
        {
            return View(new LogOnModel
                {
                    FacebookLoginUri = facebookClient.GetAccessCodeRequestUri(),
                    GoogleLoginUri = googleClient.GetAccessCodeRequestUri()
                });
        }

        /// <summary>
        /// Renders information received from authentication service.
        /// </summary>
        public ActionResult Auth(string code, string error)
        {
            //foreach (var client in clients)
            //{
            //    return View(client.GetUserInfo(client.GetAccessToken(code, error)));
            //}

            return RedirectToRoute("default");
        }
    }
}
