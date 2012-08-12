using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public interface IMailMessageParser
    {
        string GetSubject(string subject);
        ICollection<MailAddress> GetReceivers(MailMessage mailMessage);
        MailAddress GetSenderFromForwardedMail(MailMessage mailMessage);
        bool IsForwardedMail(MailMessage mailMessage);
    }
}