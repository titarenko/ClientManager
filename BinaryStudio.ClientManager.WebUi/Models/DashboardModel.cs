using System.Linq;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class DashboardModel
    {
        public IQueryable<Inquiry> inquiries { get; set; }
        public IQueryable<Person> employees { get; set; }
    }
}
