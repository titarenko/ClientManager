using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            unread.AddRange(unreadMessages.Select(message => GetInputMailMessage(message.Value)));

            client.NewMessage += (sender, args) =>
                {
                    var message = client.GetMessage(args.MessageCount - 1, false, true);
                    client.SetFlags(Flags.Seen, message);
                    unread.Add(GetInputMailMessage(message));

                    if (null != MailMessageReceived)
                        {
                            MailMessageReceived(this, args);
                        }
                };
        }

        private MailMessage GetInputMailMessage(AE.Net.Mail.MailMessage message)
        {
            return new MailMessage
            {
                Date = message.Date,
                Sender = message.From,
                Receivers = message.To,
                Subject = message.Subject,
                Body = message.Body,
                UserAgent = GetUserAgent(message.Raw)
            };
        }

        private string GetUserAgent(string raw)
        {   
            var userAgentMatch = @"\r\nUser-Agent:.*\r\n";
            var userAgentRegex = new Regex(userAgentMatch, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var matchesUserAgent = userAgentRegex.Matches(raw);
            if (matchesUserAgent.Count>0)
            {
                return matchesUserAgent[0].Value;
            }
            return "Unknown User Agent";
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
