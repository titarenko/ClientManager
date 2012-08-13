using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Parser for forwarded mail message
    /// </summary>
    public interface IMailMessageParser
    {
        /// <summary>
        /// Get original subject
        /// </summary>
        string GetSubject(string subject);

        /// <summary>
        /// Get original sender.
        /// </summary>
        /// <param name="mailMessage">Forwarded mail message</param>
        /// <returns></returns>
        MailAddress GetSender(MailMessage mailMessage);

        /// <summary>
        /// Get original body.
        /// </summary>
        /// <param name="mailMessage">Forwarded mail message</param>
        /// <returns></returns>
        string GetBody(MailMessage mailMessage);
    }
}