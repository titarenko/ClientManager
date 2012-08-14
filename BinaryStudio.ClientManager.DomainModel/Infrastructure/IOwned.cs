namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface IOwned : IIdentifiable
    {
        new int Id { get; set; }

        int OwnerId { get; set; }
    }   
}
