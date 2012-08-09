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
                return (User)HttpContext.Current.Session[ParamName];
            }
            set 
            {  
                HttpContext.Current.Session[ParamName] = value; 
            }
        }
    }
}
