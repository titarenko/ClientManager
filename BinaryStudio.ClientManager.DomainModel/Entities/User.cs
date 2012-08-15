using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class User : IIdentifiable
    {
        public int Id { get; set; }

        public string GoogleId { get; set; }

        public string FacebookId { get; set; }

        public Person RelatedUser { get; set; }

        public IList<Team> Teams { get; set; }
    }
}
