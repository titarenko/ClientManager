using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<TaggedInquiryViewModel> Inquiries { get; set; }

        public TagViewModel Tag { get; set;}
    }
}