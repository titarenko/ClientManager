using System.Collections.Generic;
using System.Linq;
using AE.Net.Mail;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class AeEventBasedEmailClient : IEmailClient
    {
        private List<MailMessage>  unread = new List<MailMessage>();

        private ImapClient client;

        public AeEventBasedEmailClient(IConfiguration configuration)
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
            client.NewMessage += (sender, args) =>
                                     {
                                         if (args.MessageCount > 0)
                                         {
                                             unread.AddRange(client.SearchMessages(SearchCondition.New())
                                                                 .Select(message => new MailMessage
                                                                                        {
                                                                                            Date = message.Value.Date,
                                                                                            Sender =
                                                                                                message.Value.Sender,
                                                                                            Receivers = message.Value.To,
                                                                                            Subject =
                                                                                                message.Value.Subject,
                                                                                            Body = message.Value.Body
                                                                                        }));
                                         }
                                     };
        }

        public IEnumerable<MailMessage> GetUnreadMessages() //renew count of unread messages
        {
            var temp = unread;
            unread = new List<MailMessage>();
            return temp;
        }
    }
}
