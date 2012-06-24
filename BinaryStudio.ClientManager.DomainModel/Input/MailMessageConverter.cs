using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageConverter
    {
        /// <summary>
        /// This method converts Input.MailMessage type into Entities.MailMessage type
        /// </summary>
        /// <param name="message">Input.MailMessage type of mail message </param>
        /// <param name="sender">Represents sender as Person class</param>
        /// <param name="receivers">Represents reveivers as ICollection of Person class</param>
        /// <returns>Entities.MailMessage type of mail message</returns>
        public Entities.MailMessage ConvertMailMessageFromInputTypeToEntityType(Input.MailMessage message, Person sender, ICollection<Person> receivers)
        {
            var returnMessage = new Entities.MailMessage();
            returnMessage.Sender = sender;
            returnMessage.Date = message.Date;
            returnMessage.Subject = message.Subject;
            returnMessage.Receivers = receivers;
            return returnMessage;         
        }
    }
}
