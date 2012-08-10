using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using AE.Net.Mail.Imap;
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

        private readonly IEmailClient emailClient;

        public MailMessagePersister(IRepository repository, IEmailClient emailClient, IInquiryFactory inquiryFactory)
        {
            this.repository = repository;

            this.inquiryFactory = inquiryFactory;

            this.emailClient = emailClient;

            emailClient.OnObtainingMessage += Proceed;

            Proceed(emailClient, new MessageEventArgs());
        }

        public void Proceed(object sender, EventArgs args)
        {
            var unreadMessages = emailClient.GetUnreadMessages();
            foreach (var message in unreadMessages)
            {
                var convertedMessage = Convert(message);

                repository.Save(convertedMessage);

                CreateInquiry(convertedMessage);
            }
        }

        private void CreateInquiry(Entities.MailMessage convertedMessage)
        {
            if (convertedMessage.Sender.Role == PersonRole.Client &&
                !repository.Query<Inquiry>(x => x.Source)
                     .Select(x => x.Source)
                     .Any(convertedMessage.SameMessagePredicate))
            {
                var inquiry = inquiryFactory.CreateInquiry(convertedMessage);
                repository.Save(inquiry);
            }
        }

        /// <summary>
        /// Converts Input.MailMessage to Entities.MailMessage.
        /// If sender or receivers isn't exist then they will be added to repository
        /// If email forwarded then sender will be taken from body <see cref="MailMessageParser"/>
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

            var mailMessageParser = new MailMessageParser(mailMessage);
            // If mail message is forwarded then Receiver will be person who forward mail and Sender taken from body
            if (mailMessageParser.IsForwardedMail())
            {             
                mailMessage.Receivers = new List<MailAddress> { mailMessage.Sender };
                mailMessage.Sender = mailMessageParser.GetSenderFromForwardedMail();
            }

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
