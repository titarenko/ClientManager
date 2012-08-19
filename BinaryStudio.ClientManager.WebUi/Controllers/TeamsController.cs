using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using BinaryStudio.ClientManager.WebUi.Models;

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
            var user = CurrentUser;
            user.Teams = repository
                .Query<Team>(x => x.Users)
                .Where(x => x.Users.Any(y => y.Id == user.Id))
                .ToList();

            return View(new TeamsViewModel
            {
                User = CurrentUser,
                Employees = repository
                    .Query<Person>()
                    .Where(x => x.Role == PersonRole.Employee)
                    .Select(x => new EmployeeViewModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    })
            });
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
            team.Users.Add(user);
            repository.Save(team);
            CurrentUser = user;
        }

        [HttpPost]
        public void AddUser(int personId, int teamId)
        {
            var user = repository.Query<User>().FirstOrDefault(x => x.RelatedPerson.Id == personId);
            var team = repository.Get<Team>(teamId);

            if (user == null || team == null)
                throw new ModelIsNotValidException();

            team.Users.Add(user);
            user.Teams.Add(team);
            repository.Save(team);
            CurrentUser = user;
        }

        [HttpPost]
        public void RemoveUser(int userId, int teamId)
        {
            var user = repository.Get<User>(userId);
            var team = repository.Get<Team>(teamId, x => x.Users);

            if (user == null || team == null)
                throw new ModelIsNotValidException();

            team.Users.Remove(user);
            user.Teams.Remove(team);
            if (user.CurrentTeam.Id==team.Id)
            {
                user.CurrentTeam = null;
            }
            repository.Save(team);
            
            CurrentUser = user;
            //TODO: ask what should we do with associated inquiries?
            //if (!team.Users.Any())
            //    repository.Delete(team);
        }

        [HttpPost]
        public void MakeTeamCurrent(int teamId)
        {
            var team = repository.Get<Team>(teamId);
            var user = CurrentUser;
            user.CurrentTeam = team;
            CurrentUser = user;
        }

        private User CurrentUser
        {
            get
            {
                return repository.Get<User>(appContext.User.Id, x => x.RelatedPerson, x => x.Teams);
            }
            set
            {
                appContext.CurrentUser = value;
                repository.Save(value);
            }
        }
    }
}