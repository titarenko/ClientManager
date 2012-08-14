using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Represents team of users
    /// </summary>
    public class Team : IIdentifiable
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the team
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of users who belong to the team
        /// </summary>
        public IList<User> Users { get; set; }
    }
}
