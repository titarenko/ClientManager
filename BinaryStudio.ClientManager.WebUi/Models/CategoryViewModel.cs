using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<TaggedInquiryViewModel> Inquiries { get; set; }

        public Tag Tag { get; set;}
    }
}