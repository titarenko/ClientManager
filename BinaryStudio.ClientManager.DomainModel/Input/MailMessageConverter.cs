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

        public MailMessageConverter(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
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
