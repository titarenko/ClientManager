using System.Linq;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class DashboardModel
    {
        public IQueryable<Inquiry> Inquiries { get; set; }
        public IQueryable<Person> Employees { get; set; }
    }
}
