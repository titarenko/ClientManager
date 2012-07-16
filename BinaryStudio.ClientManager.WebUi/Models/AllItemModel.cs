using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllItemModel
    {
        public IList<InquiryViewModel> Inquiries { get; set; }

        public string Tag { get; set; }
    }
}