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


        public bool Equals(MailMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Date.Equals(Date) && Equals(other.Sender, Sender) && Equals(other.Receivers, Receivers) && Equals(other.Subject, Subject) && Equals(other.Body, Body) && other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (MailMessage)) return false;
            return Equals((MailMessage) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Date.GetHashCode();
                result = (result*397) ^ (Sender != null ? Sender.GetHashCode() : 0);
                result = (result*397) ^ (Receivers != null ? Receivers.GetHashCode() : 0);
                result = (result*397) ^ (Subject != null ? Subject.GetHashCode() : 0);
                result = (result*397) ^ (Body != null ? Body.GetHashCode() : 0);
                result = (result*397) ^ Id;
                return result;
            }
        }
    }
}