using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllItemModel
    {
        public IEnumerable<AllInquiryViewModel> Inquiries { get; set; }

        public string Tag { get; set; }
    }
}