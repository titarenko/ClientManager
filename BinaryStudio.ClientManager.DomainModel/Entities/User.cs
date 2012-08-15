using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class User : IIdentifiable
    {
        public User()
        {
            Teams=new List<Team>();
        }

        public int Id { get; set; }

        public string GoogleCode { get; set; }

        public string FacebookCode { get; set; }

        public Person RelatedUser { get; set; }

        public IList<Team> Teams { get; set; }

        public Team CurrentTeam { get; set; }
    }
}
