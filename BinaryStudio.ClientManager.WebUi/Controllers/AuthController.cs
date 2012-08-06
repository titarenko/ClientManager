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
        private readonly IEnumerable<IClient> clients;
        private readonly IRepository repository;

        public AuthController(IRepository repository, IEnumerable<IClient> clients)
        {
            this.repository = repository;
            this.clients = clients;
        }

        /// <summary>
        /// Renders page with login link.
        /// </summary>
        public ActionResult LogOn()
        {
            return View(new LogOnModel
                {
                    LoginUris = clients.Select(x => x.GetAccessCodeRequestUri())
                });
        }

        /// <summary>
        /// Renders information received from authentication service.
        /// </summary>
        public ActionResult Auth(string code, string error)
        {
            foreach (var client in clients)
            {
                return View(client.GetUserInfo(client.GetAccessToken(code, error)));
            }

            throw new ApplicationException("oO"); //TODO fix
        }
    }
}
