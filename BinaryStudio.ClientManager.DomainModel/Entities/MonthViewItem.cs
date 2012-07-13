using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Class that represents model of month view item.
    /// </summary>
    public class MonthViewItem : IIdentifiable
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Date of deadline
        /// </summary>
        public DateTime ReferenceDate { get; set; }
        
        /// <summary>
        /// Author of inquiry.
        /// </summary>
        public Person Client { get; set; }
    }
}
