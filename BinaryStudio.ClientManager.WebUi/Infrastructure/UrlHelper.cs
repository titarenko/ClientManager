using System.Web;

namespace BinaryStudio.ClientManager.WebUi.Infrastructure
{
    public class UrlHelper : IUrlHelper
    {
        private readonly System.Web.Mvc.UrlHelper helper;

        public UrlHelper()
        {
            helper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public string Action(string action, string controller)
        {
            return helper.Action(action, controller);
        }
    }
}