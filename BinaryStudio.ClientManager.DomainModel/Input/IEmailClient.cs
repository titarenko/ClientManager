using System.Collections.Generic;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Provides access to email server by exposing methods for interacting with it.
    /// </summary>
    public interface IEmailClient
    {
        /// <summary>
        /// Obtains messages from the server.
        /// </summary>
        /// <returns>
        /// Collection of messages. 
        /// Empty collection is returned if there are no new messages on the server.
        /// </returns>
        IEnumerable<MailMessage> GetUnreadMessages();
    }
}