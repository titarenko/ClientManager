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
            var user = GetCurrentUser();
            user.Teams = repository
                .Query<Team>(x => x.Users)
                .Where(x => x.Users.Any(y => y.Id == user.Id))
                .ToList();
            foreach (var team in user.Teams)
            {
                team.Users =
                    repository.Query<User>(x => x.Teams, x => x.RelatedPerson).Where(
                        x => x.Teams.Any(z => z.Id == team.Id)).ToList();
            }
            

            return View(new TeamsViewModel
            {
                User = user,
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

        public ViewResult CurrentTeamAndUser()
        {
            return View(GetCurrentUser());
        }

        [HttpPost]
        public void CreateTeam(string name)
        {
            if (repository.Query<Team>().Any(x => x.Name == name))
                throw new ModelIsNotValidException();

            var team = new Team { Name = name };
            var user = GetCurrentUser();

            user.Teams.Add(team);
            user.CurrentTeam = user.Teams.Last();
            team.Users.Add(user);
            repository.Save(team);
            SaveCurrentUserAndCurrentTeam(user);
        }

        [HttpPost]
        public void AddUser(int personId, int teamId)
        {
            var user = repository.Query<User>(x=>x.RelatedPerson,x=>x.Teams).FirstOrDefault(x => x.RelatedPerson.Id == personId);
            var team = repository.Get<Team>(teamId,x => x.Users);
            //Cant get users related persons so i did it manually   
            //team.Users = repository.Query<User>(x => x.RelatedPerson, x => x.Teams).Where(x => x.Teams.Any(z=>z.Id==team.Id)).ToList();

            if (user == null || team == null)
                throw new ModelIsNotValidException();
            if (team.Users.Contains(user))
            {
                throw new ModelIsNotValidException();
            }
            team.Users.Add(user);
            user.Teams.Add(team);
            repository.Save(team);
            repository.Save(user);
        }

        [HttpPost]
        public void RemoveUser(int userId, int teamId)
        {
            var user = repository.Get<User>(userId, x=>x.Teams, x=>x.RelatedPerson);
            var team = repository.Get<Team>(teamId, x => x.Users);

            if (user == null || team == null)
                throw new ModelIsNotValidException();

            team.Users.Remove(user);
            user.Teams.Remove(team);
            if (user.SafeGet(x=>x.CurrentTeam.Id)==team.Id)
            {
                user.CurrentTeam = null;
            }
            repository.Save(team);
            
            SaveCurrentUserAndCurrentTeam(user);
            //TODO: ask what should we do with associated inquiries?
            //if (!team.Users.Any())
            //    repository.Delete(team);
        }

        [HttpPost]
        public void MakeTeamCurrent(int teamId)
        {
            var team = repository.Get<Team>(teamId);
            var user = GetCurrentUser();
            user.CurrentTeam = team;
            SaveCurrentUserAndCurrentTeam(user);
        }

        private void SaveCurrentUserAndCurrentTeam(User value)
        {
            appContext.CurrentTeam = value.CurrentTeam;
            repository.Save(value);
        }

        private User GetCurrentUser()
        {
            return repository.Get<User>(appContext.User.Id, x => x.RelatedPerson, x => x.Teams);
        }
    }
}