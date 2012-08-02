using System.Collections.Generic;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class WeekViewModel
    {
        public IList<Person> Employees { get; set; }

        public IList<Tag> Tags { get; set; }

        public IEnumerable<WeekItemViewModel> Days { get; set; }
    }
}