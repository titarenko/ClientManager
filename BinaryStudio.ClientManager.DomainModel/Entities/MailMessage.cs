using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class MailMessage
    {
        public DateTime Date { get; set; }

        public MailAddress Sender { get; set; }

        public ICollection<MailAddress> Receivers { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}