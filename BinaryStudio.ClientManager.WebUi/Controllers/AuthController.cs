using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.WebUi.Models;
using OAuth2.Client;
using OAuth2.Models;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepository repository;
        private readonly GoogleClient googleClient;
        private readonly FacebookClient facebookClient;
        private readonly IAppContext appContext;

        public AuthController(IRepository repository, IAppContext appContext, GoogleClient googleClient, FacebookClient facebookClient)
        {
            this.repository = repository;
            this.googleClient = googleClient;
            this.facebookClient = facebookClient;
            this.appContext = appContext;
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
        public ActionResult GoogleAuth(string code, string error)
        {
            UserInfo userInfo;
            try
            {
                userInfo = googleClient.GetUserInfo(googleClient.GetAccessToken(code, error));
            }
            catch
            {
                return RedirectToRoute("LogOn");
            }
            

            var savedUser = repository.Query<User>().SingleOrDefault(x => x.GoogleCode == code);

            if (null != savedUser)
            {
                appContext.User = savedUser;
            }
            else
            {
                var newUser = new User
                                  {
                                      GoogleCode = code,
                                      RelatedUser = repository.Query<Person>().SingleOrDefault(x => x.Email == userInfo.Email)
                                  };

                repository.Save(newUser);

                appContext.User = newUser;
            }


            return RedirectToRoute("default");
        }



        public ActionResult FacebookAuth(string code, string error)
        {
            throw new NotImplementedException();
        }
    }
}
