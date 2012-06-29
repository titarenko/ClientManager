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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="objectToCompare">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object objectToCompare)
        {
            var mailMessageToCompare = objectToCompare as MailMessage;
            if (mailMessageToCompare != null)
            {
                var collectionOfPersonsComparer = new CollectionOfPersonsComparer();
                if (Date == mailMessageToCompare.Date &&
                        Sender.Equals(mailMessageToCompare.Sender) &&
                        collectionOfPersonsComparer.Equals(Receivers,mailMessageToCompare.Receivers) &&
                        Subject == mailMessageToCompare.Subject &&
                        Body == mailMessageToCompare.Body &&
                        Id == mailMessageToCompare.Id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing 
        /// algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Id;
        }
    }
}