using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// gives access to personal data of the current user, who logged on.
    /// </summary>
    public interface IAppContext
    {
        User User { get; set; }
    }
}
