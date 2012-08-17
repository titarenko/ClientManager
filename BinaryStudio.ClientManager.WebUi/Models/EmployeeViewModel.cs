using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string label
        {
            get { return "{0} {1}".Fill(FirstName, LastName); }
        }
    }
}