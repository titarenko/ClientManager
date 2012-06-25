namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface IIdentifiable<out TKey>
    {
        TKey Id { get; }
    }
}