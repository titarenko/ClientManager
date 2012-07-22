using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllInquiriesInquiryItemViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return "{0} {1}".Fill(FirstName, LastName);
            }
        }

        public string Subject { get; set; }

        public IList<Tag> Tags { get; set; }
    }
}