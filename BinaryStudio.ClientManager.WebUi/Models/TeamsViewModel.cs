using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class TeamsViewModel
    {
        public User User { get; set; }

        public IEnumerable<EmployeeViewModel> Employees { get; set; }
    }
}