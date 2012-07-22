using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllInquiriesCategoryItemViewModel
    {
        public IEnumerable<AllInquiriesInquiryItemViewModel> Inquiries { get; set; }

        public Tag Tag { get; set;}
    }
}