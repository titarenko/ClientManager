using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AdminViewModel
    {
        public IEnumerable<Person> Employees { get; set; }
        public IEnumerable<InquiryViewModel> Inquiries { get; set; }
    }
}