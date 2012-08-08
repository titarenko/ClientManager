using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// saves received messages into repository
    /// </summary>
    public class MailMessagePersister
    {
        private readonly IRepository repository;

        private readonly IInquiryFactory inquiryFactory;

        public MailMessagePersister(IRepository repository, IEmailClient emailClient, IInquiryFactory inquiryFactory)
        {
            this.repository = repository;

            this.inquiryFactory = inquiryFactory;
            
            emailClient.OnObtainingMessage += (sender, args) =>
                {
                    foreach (var message in emailClient.GetUnreadMessages())
                    {
                        var convertedMessage = Convert(message);

                        if (!repository.Query<Entities.MailMessage>().Any(convertedMessage.SameMessagePredicate())) //if this is a new message
                        {
                            var person = repository.Query<Person>(x => x.RelatedMails)
                                .First(x => x.Email == convertedMessage.Sender.Email);
                            person.RelatedMails.Add(convertedMessage);
                            repository.Save(person);

                            this.inquiryFactory.CreateInquiry(convertedMessage);
                        }
                    }
                };
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
                Receivers = new List<Person>()
            };
            //find a Sender in Repository
            var sender = repository.Query<Person>().FirstOrDefault(x => x.Email == mailMessage.Sender.Address);

            returnMessage.Sender = sender ?? AddNewPersonToRepository(mailMessage.Sender, mailMessage.Date);

            //find Receivers in repository
            foreach (var receiver in mailMessage.Receivers)
            {
                var currentReceiver = repository.Query<Person>().FirstOrDefault(x => x.Email == receiver.Address);

                returnMessage.Receivers.Add(currentReceiver ?? AddNewPersonToRepository(receiver, mailMessage.Date));
            }

            return returnMessage;
        }

        /// <summary>
        /// Create new person in repository
        /// </summary>
        /// <param name="mailOfPerson">Mail address and name of person</param>
        /// <param name="dateOfIncomingMail">Date when mail is arrived</param>
        /// <returns>Person that was added to repository</returns>
        private Person AddNewPersonToRepository(MailAddress mailOfPerson, DateTime dateOfIncomingMail)
        {
            //Split name of client into first name and last name
            char[] separator = { ' ' };
            var personNameList = mailOfPerson.DisplayName.Split(separator).ToList();

            //add person to Repository
            var person = new Person
            {
                CreationDate = dateOfIncomingMail,
                Email = mailOfPerson.Address,
                FirstName = personNameList.Count >= 1 ? personNameList[0] : "",
                LastName = personNameList.Count >= 2 ? personNameList[1] : "",
                Role = PersonRole.Client
            };
            repository.Save(person);
            return person;
        }
    }
}
