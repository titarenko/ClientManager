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
            return View(CurrentUser);
        }

        [HttpPost]
        public void CreateTeam(string name)
        {
            if (repository.Query<Team>().Any(x => x.Name == name))
                throw new ModelIsNotValidException();

            var team = new Team { Name = name };
            var user = CurrentUser;

            user.Teams.Add(team);
            user.CurrentTeam = user.Teams.Last();
            repository.Save(user);
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
        }

        [HttpPost]
        public void RemoveUser(int userId, int teamId)
        {
            var user = repository.Get<User>(userId);
            var team = repository.Get<Team>(teamId);

            if (user == null || team == null)
                throw new ModelIsNotValidException();

            team.Users.Remove(user);

            //TODO: ask what should we do with associated inquiries?
            if (!team.Users.Any())
                repository.Delete(team);
            else
                repository.Save(team);
        }

        private User CurrentUser
        {
            get
            {
                return repository.Get<User>(appContext.User.Id, x => x.RelatedPerson, x => x.Teams);
            }
        }
    }
}