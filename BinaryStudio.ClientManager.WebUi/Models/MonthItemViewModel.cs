using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthItemViewModel
    {
        public int WeekNumber { get; set; }

        public IEnumerable<WeekItemViewModel> Days { get; set; }
    }
}