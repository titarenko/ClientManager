namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface IOwned:IIdentifiable
    {
        int Id { get; set; }
        int OwnerId { get; set; }

    }
}
