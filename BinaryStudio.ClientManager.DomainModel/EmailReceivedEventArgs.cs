using System;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel
{
    public class EmailReceivedEventArgs : EventArgs
    {
        public MailMessage Mail { get; set; }
    }
}