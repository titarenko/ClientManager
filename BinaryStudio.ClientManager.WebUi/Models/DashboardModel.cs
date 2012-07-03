using System.Collections.Generic;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class DashboardModel
    {
        /// <summary>
        /// incoming inquiries
        /// </summary>
        public IList<Inquiry> Inquiries { get; set; }

        /// <summary>
        /// requests in the process
        /// </summary>
        public IList<Inquiry> InProgress { get; set; }

        /// <summary>
        /// awaiting reply
        /// </summary>
        public IList<Inquiry> WaitingForReply { get; set; } 

        /// <summary>
        /// assigned employee
        /// </summary>
        public SelectList Employees { get; set; }
    }
}
