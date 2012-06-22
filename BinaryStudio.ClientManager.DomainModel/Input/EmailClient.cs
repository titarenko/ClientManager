using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using ActiveUp.Net.Mail;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Client, which connect to mail server
    /// </summary>
    public class EmailClient : IEmailClient
    {
        private readonly Imap4Client _client = null;

        protected Imap4Client Client
        {
            get { return _client; }
        }

        public EmailClient(string mailServer, int port,
            bool ssl, string login, string password)
        {
            _client = new Imap4Client();
            if (ssl)
                Client.ConnectSsl(mailServer, port);
            else
                Client.Connect(mailServer, port);
            Client.Login(login, password);
        }

        /// <summary>
        /// Gets all unread messages.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MailMessage> GetUnreadMessages()
        {
            return GetMails("Input", "ALL").Cast<MailMessage>();
        }

        /// <summary>
        /// Gets the mails.
        /// </summary>
        /// <param name="mailBox">The mail box.</param>
        /// <param name="seachPhrase">The seach phrase.</param>
        /// <returns></returns>
        private MessageCollection GetMails(string mailBox, string seachPhrase)
        {
            Mailbox box = Client.SelectMailbox(mailBox);
            MessageCollection messages = box.SearchParse(seachPhrase);
            return messages;
        }
    }
}