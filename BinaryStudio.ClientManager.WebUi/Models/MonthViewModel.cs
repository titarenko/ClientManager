using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthViewModel
    {
        public IList<Inquiry> Inquiries { get; set; }
        public List<Person> Clients { get; set; }
        public IEnumerable<MonthViewItem> MonthViewItems { get; set; }
    }
}