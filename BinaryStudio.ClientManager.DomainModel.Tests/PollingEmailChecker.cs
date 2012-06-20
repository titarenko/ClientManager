using System;

namespace BinaryStudio.ClientManager.DomainModel.Tests
{
    public class PollingEmailChecker
    {
        private readonly IEmailClient emailClient;

        public PollingEmailChecker(Timer timer, IEmailClient emailClient)
        {
            timer.OnTick += OnTick;
            this.emailClient = emailClient;
        }

        private void OnTick(object sender, EventArgs eventArgs)
        {
            var messages = emailClient.GetMessages();
            foreach (var message in messages)
            {
                if (EmailReceived != null)
                {
                    EmailReceived(this, new EmailReceivedEventArgs
                    {
                        Mail = message
                    });
                }
            }
        }

        public event EventHandler<EmailReceivedEventArgs> EmailReceived;
    }
}