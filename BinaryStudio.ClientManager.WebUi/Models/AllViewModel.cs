using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllViewModel
    {
        public IList<Inquiry> Inquiries { get; set; }

        public string Tag { get; set; }
    }
}