using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthViewModel
    {
        public int SkippedDays { get; set; }

        public int StartingWeek { get; set; }

        public int FinishingWeek { get; set; }

        public string MonthName { get; set; }

        public IEnumerable<MonthItemViewModel> Weeks { get; set; }
    }
}