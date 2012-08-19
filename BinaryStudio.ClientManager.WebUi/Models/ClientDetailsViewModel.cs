using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class ClientDetailsViewModel
    {
        public Person Client { set; get; }
        public IEnumerable<Inquiry> Inquiries { get; set; }
    }
}