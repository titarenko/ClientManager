using System.Web.Mvc;
using System.Web.Script.Serialization;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.WebUi.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString SkypeLink(this HtmlHelper helper, string phone, string text = null)
        {
            var tag = new TagBuilder("a");
            tag.AddCssClass("icon-skype");
            tag.Attributes.Add("href", "skype:{0}?call".Fill(phone));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString Email(this HtmlHelper helper, string email, string text = null)
        {
            var tag = new TagBuilder("a");
            tag.AddCssClass("email");
            tag.Attributes.Add("href", "mailto:{0}".Fill(email));
            tag.SetInnerText(text ?? email);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ToJson(this HtmlHelper helper, object model)
        {
            var serializer = new JavaScriptSerializer();
            return MvcHtmlString.Create(serializer.Serialize(model));
        }
    }
}