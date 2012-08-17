using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// adds tagging to inquiry
    /// </summary>
    public class Tag : IIdentifiable, IOwned
    {
        public Tag()           
        {
            Inquiries=new List<Inquiry>();        
        }

        /// <summary>
        /// Id of tag
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tag name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Inquiries with that tag
        /// </summary>
        public IList<Inquiry> Inquiries { get; set; }

        /// <summary>
        /// Owner of tag
        /// </summary>
        public Team Owner { get; set; }
    }
}
