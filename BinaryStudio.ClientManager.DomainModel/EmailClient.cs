using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel
{
    public class EmailClient : IEmailClient
    {
        public IEnumerable<MailMessage> GetMessages()
        {
            throw new NotImplementedException();
        }
    }
}