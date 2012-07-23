using System;
using System.Collections.Generic;
using System.Globalization;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthItemViewModel
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int WeekNumber {get
            {
               return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Date, CalendarWeekRule.FirstDay,
                                                                 DayOfWeek.Monday);
            }
        }

        public IEnumerable<InquiryViewModel> Inquiries { get; set; }
    }
}