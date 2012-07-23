using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllInquiriesViewModel
    {
        public IEnumerable<AllInquiriesCategoryItemViewModel> Categories { get; set; }
    }
}