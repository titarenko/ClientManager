using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public interface IEmailClient
    {
        IEnumerable<MailMessage> GetMessages();
    }
}