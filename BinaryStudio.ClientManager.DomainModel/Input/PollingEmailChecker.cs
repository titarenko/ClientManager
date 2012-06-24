using System;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Receives emails by periodic polling of server.
    /// </summary>
    public class PollingEmailChecker
    {
        private readonly IEmailClient emailClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PollingEmailChecker"/> class.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="emailClient">The email client.</param>
        public PollingEmailChecker(Timer timer, IEmailClient emailClient)
        {
            timer.OnTick += OnTick;
            this.emailClient = emailClient;
        }

        /// <summary>
        /// Occurs when new email is received from server.
        /// </summary>
        public event EventHandler<EmailReceivedEventArgs> EmailReceived;

        private void OnTick(object sender, EventArgs eventArgs)
        {
            foreach (var message in emailClient.GetUnreadMessages())
            {
                if (EmailReceived != null)
                {
                    EmailReceived(this, new EmailReceivedEventArgs
                    {
                        Message = message
                    });
                }
            }
        }
    }
}