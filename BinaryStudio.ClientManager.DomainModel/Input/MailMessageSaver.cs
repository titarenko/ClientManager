﻿using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

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

        public MailMessageSaver(IRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            converter = new MailMessageConverter(repository);
            emailClient = new AeEmailClient(configuration);
            
            emailClient.OnObtainingMessage += (sender, args) =>
                {
                    foreach (var message in emailClient.GetUnreadMessages())
                    {
                        var convertedMessage = converter.Convert(message);
                        if (null == repository.Query<Entities.MailMessage>().SingleOrDefault(x => x.Body == convertedMessage.Body &&
                            x.Subject == convertedMessage.Subject &&
                            x.Sender.Email == convertedMessage.Sender.Email))
                        {
                            repository.Save(convertedMessage);
                        }
                    }
                };
        }
    }
}