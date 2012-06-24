using System;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Arguments of EmailReceived event (see <see cref="PollingEmailChecker"/>).
    /// </summary>
    public class EmailReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received message instance.
        /// </summary>
        public MailMessage Message { get; set; }
    }
}