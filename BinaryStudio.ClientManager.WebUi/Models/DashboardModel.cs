using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class DashboardModel
    {
        public IQueryable<Inquiry> Inquiries { get; set; }
        public List<SelectListItem> Employees { get; set; }
    }
}
