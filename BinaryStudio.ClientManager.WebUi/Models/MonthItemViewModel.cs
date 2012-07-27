using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthItemViewModel
    {
        public IEnumerable<WeekItemViewModel> Days { get; set; }
    }
}