using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthViewModel
    {
        public IList<Inquiry> Inquiries { get; set; }
        public List<Person> Clients { get; set; }
        public IList<Inquiry> MonthViewItems { get; set; }
        //public IRepository<MonthViewItem> MonthViewItems { get; set; }
    }
}