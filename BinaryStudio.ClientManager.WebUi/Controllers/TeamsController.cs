using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly IRepository repository;

        private readonly IAppContext appContext;

        public TeamsController(IRepository repository, IAppContext appContext)
        {
            this.repository = repository;
            this.appContext = appContext;
        }


        public ViewResult Index()
        {
            return View(GetCurrentUser.Teams);
        }


        [HttpPost]
        public void CreateTeam(string name)
        {
            if (repository.Query<Team>().Any(x => x.Name == name))
                throw new ModelIsNotValidException();

            var team = new Team { Name = name };
            team.Users.Add(GetCurrentUser);
            repository.Save(team);
        }

        [HttpPost]
        public void AddUser(int userId, int teamId)
        {
            var user = repository.Get<User>(userId);
            var team = repository.Get<Team>(teamId);

            if (user == null || team == null)
                throw new ModelIsNotValidException();

            team.Users.Add(user);
            repository.Save(team);

            appContext.User.Teams.First(x => x.Id == teamId).Users.Add(user);
        }

        [HttpPost]
        public void RemoveUser(int userId, int teamId)
        {
            var user = repository.Get<User>(userId);
            var team = repository.Get<Team>(teamId);

            if (user == null || team == null)
                throw new ModelIsNotValidException();

            team.Users.Remove(user);
            user.Teams.Remove(team);
            repository.Save(team);
            repository.Save(user);

            //TODO: ask what should we do with inquiries?
            if (!team.Users.Any())
                repository.Delete(team);

            //if (userId == appContext.User.Id)
            //    appContext.User.Teams.Remove(team);
            //else
            //    appContext.User.Teams.First(x => x.Id == teamId).Users.Remove(user);
        }


        private User GetCurrentUser
        {
            get { return repository.Get<User>(appContext.User.Id, x => x.Teams); }
        }
    }
}