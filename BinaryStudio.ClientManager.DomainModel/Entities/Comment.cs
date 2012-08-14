using System;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class Comment : IOwned
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Owner of the comment
        /// </summary>
        public Team Owner { get; set; }

        /// <summary>
        /// Text of the comment
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Creation date of the comment
        /// </summary>
        public DateTime Date { get; set; }
    }
}