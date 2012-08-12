using System.Web;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class AppContext : IAppContext
    {
        private readonly ISession session;
        private readonly IRandomToken randomToken;

        public AppContext(ISession session, IRandomToken randomToken)
        {
            this.session = session;
            this.randomToken = randomToken;
        }

        private const string Separetor = "##";

        private const string ParamName = "User";

        public User User
        {
            get
            {
                return HttpContext.Current.SafeGet(x => x.Session[ParamName]) as User;
            }
            set 
            {  
                HttpContext.Current.Session[ParamName] = value; 
            }
        }
    }
}
