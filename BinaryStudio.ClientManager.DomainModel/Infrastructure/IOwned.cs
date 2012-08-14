using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface IOwned : IIdentifiable
    {
        int Id { get; set; }

        Team Owner { get; set; }
    }
}
