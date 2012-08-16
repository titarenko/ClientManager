using System;
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
            if (null != appContext.User)
            {
                return RedirectToAction("Week", "Inquiries");
            }

            return View(new LogOnModel
                {
                    FacebookLoginUri = facebookClient.GetAccessCodeRequestUri(),
                    GoogleLoginUri = googleClient.GetAccessCodeRequestUri()
                });
        }

        /// <summary>
        /// Renders information received from authentication service.
        /// </summary>
        public ActionResult GoogleAuth(string code, string error) //TODO refactor this
        {
            UserInfo userInfo;
            try
            {
                userInfo = googleClient.GetUserInfo(googleClient.GetAccessToken(code, error));
            }
            catch
            {
                return RedirectToAction("LogOn");
            }
            
            var user = repository.Query<User>(x => x.RelatedPerson, x => x.Teams)
                .SingleOrDefault(x => x.GoogleId == userInfo.Id);

            if (null == user)
            {
                user = new User
                            {
                                GoogleId = userInfo.Id,
                                RelatedPerson = repository.Query<Person>().SingleOrDefault(x => x.Email == userInfo.Email)
                            };
                if (null == user.RelatedPerson)
                {
                    var person = new Person
                                     {
                                         Email = userInfo.Email,
                                         FirstName = userInfo.FirstName,
                                         LastName = userInfo.LastName,
                                         Role = PersonRole.Employee,
                                         CreationDate = DateTime.Now
                                     };
                    user.RelatedPerson = person;
                    repository.Save(person);
                }
                repository.Save(user);
            }

            appContext.User = user;

            return RedirectToRoute("Default");
        }


        public ActionResult LogOff()
        {
            appContext.User = null;
            return RedirectToAction("LogOn");
        }


        public ActionResult FacebookAuth(string code, string error)
        {
            throw new NotImplementedException();
        }
    }
}
