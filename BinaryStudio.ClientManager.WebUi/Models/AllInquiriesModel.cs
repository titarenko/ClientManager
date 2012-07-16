using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AllInquiriesModel
    {
        public IList<Inquiry> Inquiries { get; set; }

        public IList<Tag> Tags { get; set; }
    }
}