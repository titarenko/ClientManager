using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface IAppContext
    {
        User User { get; set; }
    }
}
