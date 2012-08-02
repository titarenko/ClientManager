using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllInquiriesViewModel
    {
        public string InquiryDetailsUrl { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}