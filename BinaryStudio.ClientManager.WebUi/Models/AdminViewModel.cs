using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AdminViewModel
    {
        public IEnumerable<InquiryViewModel> Inquiries { get; set; }
    }
}