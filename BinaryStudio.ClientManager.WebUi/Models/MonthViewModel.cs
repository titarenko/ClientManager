using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthViewModel
    {
        public IEnumerable<MonthItemViewModel> Days { get; set; }
    }
}