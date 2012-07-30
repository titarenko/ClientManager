using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class MonthViewModel
    {
        public string Name { get; set; }

        public IEnumerable<MonthItemViewModel> Weeks { get; set; }

        /// <summary>
        /// within zero
        /// </summary>
        public int MaxInquiriesWithoutToggling { get; set; }
    }
}