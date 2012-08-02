using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class InquiryPartialViewModel
    {
        public InquiryViewModel Inquiry { get; set; }

        public IList<Person> Employees { get; set; }

        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// Uri for details of current inquiry
        /// </summary>
        public string DetailsUri
        {
            get
            {
                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return url.Action("Details", "Inquiries", new { id = Inquiry.Id });
            }
        }
    }
}