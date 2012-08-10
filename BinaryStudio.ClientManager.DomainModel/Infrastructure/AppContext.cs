using System.Web;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    class AppContext : IAppContext
    {
        private const string ParamName = "User";

        public User User
        {
            get
            {
                return HttpContext.Current.Session[ParamName] as User;
            }
            set 
            {  
                HttpContext.Current.Session[ParamName] = value; 
            }
        }
    }
}
