using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class User : IIdentifiable
    {
        public int Id { get; set; }

        public string AuthCode { get; set; }

        public Person RelatedUser { get; set; }
    }
}
