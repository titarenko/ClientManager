using System;
using System.Collections.Generic;
using BinaryStudio.ClientManager.WebUi.Models;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class WeekItemViewModel
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<InquiryViewModel> Inquiries { get; set; }
    }
}