using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllInquiriesViewModel
    {
        public IEnumerable<AllInquiriesCategoryItemViewModel> Categories { get; set; }
    }
}