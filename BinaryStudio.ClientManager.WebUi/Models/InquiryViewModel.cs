using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class InquiryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string PhotoUri { get; set; }

        public string Assignee { get; set; }

        /// <summary>
        /// Uri for details of current inquiry
        /// </summary>
        public string DetailsUri
        {
            get
            {
                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return url.Action("Details", "Inquiries",new {id = Id});
            }
        }
    }
}