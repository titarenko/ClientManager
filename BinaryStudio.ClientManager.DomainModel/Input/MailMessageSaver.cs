using System.Collections.Generic;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// saves received messages into repository
    /// </summary>
    public class MailMessageSaver
    {
        private readonly IRepository repository;

        private readonly IEmailClient emailClient;

        private readonly MailMessageConverter converter;

        public MailMessageSaver(IRepository repository, IEmailClient emailClient)
        {
            this.repository = repository;
            converter = new MailMessageConverter(repository);
            
            emailClient.OnObtainingMessage += (sender, args) =>
                {
                    foreach (var message in emailClient.GetUnreadMessages())
                    {
                        var convertedMessage = converter.Convert(message);
                        if (null == repository.Query<Entities.MailMessage>().SingleOrDefault(x =>
                            x.Body == convertedMessage.Body &&
                            x.Subject == convertedMessage.Subject &&
                            x.Sender.Email == convertedMessage.Sender.Email)) //if this new message
                        {
                            repository.Save(convertedMessage);

                            var person = repository.Query<Person>().SingleOrDefault(x => x.Email == convertedMessage.Sender.Email);

                            if (null == person.RelatedMails)
                            {
                                person.RelatedMails = new List<Entities.MailMessage>();
                            }
                            person.RelatedMails.Add(convertedMessage);
                            repository.Save(person);

                            if (person.Role == PersonRole.Client && null == repository.Query<Inquiry>().AsEnumerable().SingleOrDefault(x => x.Source.Equals(convertedMessage)))
                            {
                                repository.Save(new Inquiry { 
                                    Client = person, 
                                    Description = convertedMessage.Body, 
                                    Source = convertedMessage, 
                                    Subject = convertedMessage.Subject, 
                                    ReferenceDate = convertedMessage.Date});
                            }
                        }
                    }
                };
        }
    }
}
