using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageConverter
    {
        private IRepository repository;

        /// <summary>
        /// MailMessageConverter constructor with 1 parameter
        /// </summary>
        /// <param name="repository">Repository that keep database</param>
        public MailMessageConverter(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Converts MailMessage from input domain to entities domain
        /// </summary>
        /// <param name="mailMessage">Mail message of input domain</param>
        /// <returns>Mail message of entities domain</returns>
        public Entities.MailMessage ConvertMailMessageFromInputTypeToEntityType(MailMessage mailMessage)
        {
            var returnMessage = new Entities.MailMessage
                                    {
                                        Body = mailMessage.Body, 
                                        Date = mailMessage.Date, 
                                        Subject = mailMessage.Subject,
                                        Receivers=new List<Person>()
                                    };
            //find a Sender in Database
            Expression<Func<Person, object>> expr = x => x.Email == mailMessage.Sender.Address;
            var sender = repository.Query<Person>().FirstOrDefault(x => x.Email == mailMessage.Sender.Address);
            returnMessage.Sender = sender;

            //find Receivers in Database
            foreach (var receiver in mailMessage.Receivers)
            {
                var currentReceiver = repository.Query<Person>().FirstOrDefault(x => x.Email == receiver.Address);
                returnMessage.Receivers.Add(currentReceiver);
            }
            
            return returnMessage;
        }
    }
}
