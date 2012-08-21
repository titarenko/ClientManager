using System;
using System.Web;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class AppContext : IAppContext
    {
        private readonly ICache cache;
        private readonly IRandomToken randomToken;

        private const string ParamName = "User";

        public AppContext(ICache cache, IRandomToken randomToken)
        {
            this.cache = cache;
            this.randomToken = randomToken;
        }

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
                CurrentTeam = User.SafeGet(x=>x.CurrentTeam);
            }
        }

        private void SetValueToCash<T>(T value, string token, string prefix=null)
        {
            cache.Set(prefix == null ? token : prefix + token, value);
        }

        private T GetFromCash<T>(string token,string prefix=null) where T:class 
        {
            return null == token ? null : cache.Get<T>(prefix==null ? token:prefix+token);
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
                    SetValueToCash(value, token,"team");
                }
                else
                {
                    throw new Exception("Trying to make current team without logged in");
                }
            }
        }
    }
}
