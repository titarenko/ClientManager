using System.Web;
using System.Web.Security;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class AppContext : IAppContext
    {
        private const string ParamName = "User";

        public User User
        {
            get
            {
                return HttpContext.Current.SafeGet(x => x.Session[ParamName]) as User;
            }
            set 
            {  
                 FormsAuthentication.SetAuthCookie(value.RelatedUser.Email, true);
                HttpContext.Current.Session[ParamName] = value; 
            }
        }
    }
}
