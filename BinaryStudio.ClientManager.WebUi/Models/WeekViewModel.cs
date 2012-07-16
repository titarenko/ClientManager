using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class WeekViewModel
    {
        public IList<Inquiry> Inquiries { get; set; }
        public IList<Person> Employees { get; set; }

        public IEnumerable<WeekItemViewModel> Days { get; set; }
    }
}