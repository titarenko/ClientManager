using System;
using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthItemViewModel
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<InquiryBriefViewModel> Inquiries { get; set; }
    }
}