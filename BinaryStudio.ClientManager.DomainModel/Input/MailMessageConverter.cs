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
            //find a Sender in Repository
            var sender = repository.Query<Person>().FirstOrDefault(x => x.Email == mailMessage.Sender.Address);
            if (sender!=null)
            {
                returnMessage.Sender = sender;
            }
            else //if cant find sender in repository then create him.
            {
                //Split name of client into first name and last name
                char[] separator = {' '};
                var list = mailMessage.Sender.DisplayName.Split(separator).ToList();
                if (list.Count!=2)
                {
                    if (list.Count==1) //if sender havent last name
                    {
                        list.Add("");
                    }
                    else               //if sender havent specified name or specified it illegal create empty first name and last name
                    {
                        list.Clear();
                        list.Add("");
                        list.Add("");
                    }
                }

                //add person to Repository
                var addingPerson = new Person
                                           {
                                               CreationDate = mailMessage.Date,
                                               Email = mailMessage.Sender.Address,
                                               FirstName = list[0],
                                               LastName = list[1],
                                           };
                repository.Save(addingPerson);
            }

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
