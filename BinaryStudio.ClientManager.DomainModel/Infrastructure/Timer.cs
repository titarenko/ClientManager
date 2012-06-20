using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// Provides possibility to execute certain code time to time.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Defines how often OnTick event should be issued.
        /// </summary>
        public virtual TimeSpan Interval { get; set; }

        /// <summary>
        /// Enables or disables issuing OnTick events.
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Periodically occurs after specified time interval has elapsed.
        /// </summary>
        public event EventHandler OnTick;

        protected void RaiseOnTick(object sender, EventArgs e)
        {
            if (OnTick != null)
            {
                OnTick(sender, e);
            }
        }
    }
}