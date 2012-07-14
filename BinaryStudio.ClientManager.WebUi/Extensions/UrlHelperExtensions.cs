using System.Web.Mvc;

namespace BinaryStudio.ClientManager.WebUi.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string NoScript(this UrlHelper helper)
        {
            return helper.Action("NoScript", "Home");
        }

        public static string DefaultPhoto(this UrlHelper helper)
        {
            return helper.Content("~/Content/images/photo.png");
        }
    }
}