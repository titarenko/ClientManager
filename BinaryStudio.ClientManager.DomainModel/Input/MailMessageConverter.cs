using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Provides possibility to convert mail message from Input.MailMessage type to Entities.MailMessage type
    /// </summary>
    public class MailMessageConverter
    {
        /// <summary>
        /// Repository that keeps some data about persons.
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Repository that keep database</param>
        public MailMessageConverter(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Converts Input.MailMessage to Entities.MailMessage.
        /// If sender or receivers isn't exist then they will be added to repository
        /// </summary>
        /// <param name="mailMessage">Input.MailMessage type of message</param>
        /// <returns>Entities.MailMessage type of message</returns>
        public Entities.MailMessage Convert(MailMessage mailMessage)
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
                returnMessage.Sender = addNewPersonToRepository(mailMessage.Sender, mailMessage.Date);
            }

            //find Receivers in repository
            foreach (var receiver in mailMessage.Receivers)
            {
                var currentReceiver = repository.Query<Person>().FirstOrDefault(x => x.Email == receiver.Address);
                if (currentReceiver!=null)
                {
                    returnMessage.Receivers.Add(currentReceiver);
                }
                else //if cant find receiver in repository then create him.
                {
                    returnMessage.Receivers.Add(addNewPersonToRepository(receiver, mailMessage.Date));
                }
            }
            
            return returnMessage;
        }

        /// <summary>
        /// Create new person in repository
        /// </summary>
        /// <param name="mailOfPerson">Mail address and name of person</param>
        /// <param name="dateOfIncomingMail">Date when mail is arrived</param>
        /// <returns>Person that was added to repository</returns>
        private Person addNewPersonToRepository(MailAddress mailOfPerson, DateTime dateOfIncomingMail)
        {
            //Split name of client into first name and last name
            char[] separator = { ' ' };
            var personNameList = mailOfPerson.DisplayName.Split(separator).ToList();

            //add person to Repository
            var addingPerson = new Person
            {
                CreationDate = dateOfIncomingMail,
                Email = mailOfPerson.Address,
                FirstName =  personNameList.Count>=1? personNameList[0]:"",
                LastName = personNameList.Count>=2 ? personNameList[1] : ""
            };
            repository.Save(addingPerson);
            return addingPerson;
        }
    }
}
