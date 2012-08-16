using System.Web;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class AppContext : IAppContext
    {
        private readonly ISession session;
        private readonly IRandomToken randomToken;

        private const string ParamName = "User";

        public AppContext(ISession session, IRandomToken randomToken)
        {
            this.session = session;
            this.randomToken = randomToken;
        }

        public User User
        {
            get 
            { 
                var token = HttpContext.Current.Request.SafeGet(x => x.Cookies[ParamName].Value);

                return null == token ? null : this.session.Get<User>(token);
            }
            set
            {
                var token = HttpContext.Current.Request.SafeGet(x => x.Cookies[ParamName].Value) ?? this.randomToken.GetRandomToken();
                this.session.Set(token, value);
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(ParamName, token) { HttpOnly = true });
            }
        }
    }
}
