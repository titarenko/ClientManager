using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public interface IMailMessageParser
    {
        string GetSubject(string subject);
        ICollection<MailAddress> GetReceivers(MailMessage mailMessage);
        MailAddress GetSender(MailMessage mailMessage);
        bool IsForwarded(MailMessage mailMessage);
    }
}