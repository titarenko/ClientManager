using System;
using System.Collections.Generic;
using AE.Net.Mail;
using System.Linq;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Email client based on Andy Edinborough's AE.Net.Mail library. 
    /// </summary>
    /// <remarks>
    /// Link to repository https://github.com/andyedinborough/aenetmail.
    /// </remarks>
    public class AeEmailClient : IEmailClient, IDisposable
    {
        private readonly ImapClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AeEmailClient"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AeEmailClient(IConfiguration configuration)
        {
            configuration = configuration.GetSubsection("EmailClient");
            client = new ImapClient(
                configuration.GetValue("Host"),
                configuration.GetValue("Username"),
                configuration.GetValue("Password"));
        }
            
        /// <summary>
        /// Obtains messages from the server.
        /// </summary>
        /// <returns>
        /// Collection of messages.
        /// Empty collection is returned if there are no new messages on the server.
        /// </returns>
        public IEnumerable<Input.MailMessage> GetUnreadMessages()
        {
            return client
                .SearchMessages(SearchCondition.New())
                .Select(message => new Input.MailMessage
                {
                    Date = message.Value.Date,
                    Sender = message.Value.From,
                    Receivers = message.Value.To,
                    Subject = message.Value.Subject,
                    Body = message.Value.Body
                });
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            client.Dispose();
        }
    }
}