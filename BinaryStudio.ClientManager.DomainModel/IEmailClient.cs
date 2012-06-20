using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel
{
    public interface IEmailClient
    {
        IEnumerable<MailMessage> GetMessages();
    }
}