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
            return View(appContext.User.Teams);
        }

        [HttpPost]
        public void CreateTeam(string name)
        {
            if (repository.Query<Team>().Any(x => x.Name == name))
                throw new ModelIsNotValidException();

            var team = new Team { Name = name };
            team.Users.Add(appContext.User);

            appContext.User.Teams.Add(team);
            appContext.User.CurrentTeam = team;
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
            repository.Save(team);

            // What should we do with empty team's inquiries?
            //if (!team.Users.Any())
            //    repository.Delete(team);

            if (userId == appContext.User.Id)
                appContext.User.Teams.Remove(team);
            else
                appContext.User.Teams.First(x => x.Id == teamId).Users.Remove(user);
        }
    }
}