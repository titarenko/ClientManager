using System;
using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class WeekItemViewModel
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string DateString {
            get
            {
                return Date.ToString("yyyy-MM-dd");
            } 
        }

        public IEnumerable<InquiryViewModel> Inquiries { get; set; }
    }
}