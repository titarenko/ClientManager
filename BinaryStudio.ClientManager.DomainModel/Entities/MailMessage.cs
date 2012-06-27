using System;
using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Represents e-mail message of domain entity.
    /// </summary>
    public class MailMessage : IIdentifiable
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

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }
    }
}