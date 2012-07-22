using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllItemModel
    {
        public IEnumerable<AllInquiryViewModel> Inquiries { get; set; }

        public Tag Tag { get; set;}
    }
}