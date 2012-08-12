using System;
using System.Collections.Generic;
using System.Linq;
using AE.Net.Mail;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class AeEmailClient : IEmailClient, IDisposable
    {
        public event EventHandler MailMessageReceived;

        private List<MailMessage> unread = new List<MailMessage>();

        private readonly ImapClient client;

        public AeEmailClient(IConfiguration configuration)
        {
            configuration = configuration.GetSubsection("EmailClient");
            client = new ImapClient(
                configuration.GetValue("Host"),
                configuration.GetValue("Username"),
                configuration.GetValue("Password"),
                ImapClient.AuthMethods.Login,
                configuration.GetValue<int>("Port"),
                configuration.GetValue<bool>("Secure"),
                configuration.GetValue<bool>("SkipSslValidation"));

            var unreadMessages = client.SearchMessages(SearchCondition.Unseen());
            foreach (var message in unreadMessages)
            {
                client.SetFlags(Flags.Seen, message.Value);
            }

            unread.AddRange(unreadMessages.Select(message => new MailMessage
                {
                    Date = message.Value.Date,
                    Sender = message.Value.From,
                    Receivers = message.Value.To,
                    Subject = message.Value.Subject,
                    Body = message.Value.Body
                }));
            

            client.NewMessage += (sender, args) =>
                {
                    var message = client.GetMessage(args.MessageCount - 1, false, true);
                    client.SetFlags(Flags.Seen, message);
                    unread.Add(new MailMessage
                        {
                            Date = message.Date,
                            Sender = message.From,
                            Receivers = message.To,
                            Subject = message.Subject,
                            Body = message.Body,
                        });

                    if (null != MailMessageReceived)
                        {
                            MailMessageReceived(this, args);
                        }
                };
        }

        public IEnumerable<MailMessage> GetUnreadMessages() //renew count of unread messages
        {
            var temp = unread;
            unread = new List<MailMessage>();
            return temp;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
