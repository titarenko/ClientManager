using System;
using System.Threading;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// Provides possibility to execute certain code time to time.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Standart timer
        /// </summary>
        private readonly System.Threading.Timer timer;
  
        private TimeSpan interval;

        /// <summary>
        /// Periodically occurs after specified time interval has elapsed.
        /// </summary>
        public event EventHandler OnTick;

        /// <summary>
        /// Enables or disables issuing OnTick events.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Timer()
        {
            timer = new System.Threading.Timer(new TimerCallback(TimerProc));
        }

        /// <summary>
        /// Defines how often OnTick event should be issued.
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                timer.Change(new TimeSpan(0), interval);
            }
        }

        /// <summary>
        /// left for backward compatibility with FakeTimer
        /// </summary>
        protected void RaiseOnTick(object sender, EventArgs e)
        {
            if (OnTick != null)
            {
                OnTick(sender, e);
            }
        }

        /// <summary>
        /// Callback function which will be executed time to time.
        /// </summary>
        /// <param name="state">itself timer, not used yet</param>
        private void TimerProc(object state)
        {
            if (Enabled)
            {
                RaiseOnTick(state, new EventArgs());
            }
        }
    }
}