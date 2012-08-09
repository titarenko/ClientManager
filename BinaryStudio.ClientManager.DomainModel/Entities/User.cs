using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class User : IIdentifiable
    {
        public int Id { get; set; }

        public string GoogleCode { get; set; }

        public string FacebookCode { get; set; }

        public Person RelatedUser { get; set; }
    }
}
