using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Represents e-mail message domain entity.
    /// </summary>
    public class MailMessage
    {
        /// <summary>
        /// Date when message was received.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Author of this message.
        /// </summary>
        public Person Sender { get; set; }

        /// <summary>
        /// List of all receivers.
        /// </summary>
        public ICollection<Person> Receivers { get; set; }

        /// <summary>
        /// Subject of the message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body of the message.
        /// </summary>
        public string Body { get; set; }
    }
}