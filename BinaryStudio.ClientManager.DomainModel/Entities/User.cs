using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class User : IIdentifiable
    {
        public User()
        {
            Teams = new List<Team>();
        }

        public int Id { get; set; }

        public string GoogleId { get; set; }

        public string FacebookId { get; set; }

        public Person RelatedPerson { get; set; }

        public List<Team> Teams { get; set; }

        public Team CurrentTeam { get; set; }
    }
}
