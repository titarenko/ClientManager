using System;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class EmailReceivedEventArgs : EventArgs
    {
        public MailMessage Mail { get; set; }
    }
}