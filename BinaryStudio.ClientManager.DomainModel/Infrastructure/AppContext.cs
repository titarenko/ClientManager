using System;
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
        private const string ParamTeam = "Team";

        public AppContext(ISession session, IRandomToken randomToken)
        {
            this.session = session;
            this.randomToken = randomToken;
        }

        public User CurrentUser { get; set; }

        public User User
        {
            get 
            {
                var token = HttpContext.Current.Request.SafeGet(x => x.Cookies[ParamName].Value);
                return GetFromCash<User>(token);
            }
            set
            {
                var token = HttpContext.Current.Request.SafeGet(x => x.Cookies[ParamName].Value) ?? randomToken.GetRandomToken(); 
                SetValueToCash(value,token);
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(ParamName, token) { HttpOnly = true });
            }
        }

        private void SetValueToCash<T>(T value, string token, string prefix=null)
        {
            session.Set(prefix == null ? token : prefix + token, value);
        }

        private T GetFromCash<T>(string token,string prefix=null) where T:class 
        {
            return null == token ? null : session.Get<T>(prefix==null ? token:prefix+token);
        }

        public Team CurrentTeam{ 
            get
            {
                var token = HttpContext.Current.Request.SafeGet(x => x.Cookies[ParamName].Value);
                return GetFromCash<Team>(token,"team");
            }
            set
            {
                var token = HttpContext.Current.Request.SafeGet(x => x.Cookies[ParamName].Value); 
                if (token!=null)
                {
                    SetValueToCash(value, "team");
                }
                else
                {
                    throw new Exception("ne bro");
                }
            }
        }
    }
}
