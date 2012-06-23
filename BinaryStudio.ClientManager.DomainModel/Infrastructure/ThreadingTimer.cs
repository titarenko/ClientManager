using System;
using System.Threading;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// Our implementation of timer.
    /// </summary>
    public class ThreadingTimer : Timer
    {
        private readonly System.Threading.Timer _threadingTimer;
        private TimeSpan _interval;

        public ThreadingTimer()
        {
            _threadingTimer = new System.Threading.Timer( new TimerCallback(TimerProc));
        }

        /// <summary>
        /// Defines how often OnTick event should be issued.
        /// </summary>
        public override TimeSpan Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
                _threadingTimer.Change(new TimeSpan(0), _interval);
            }
        }
        
        /// <summary>
        /// Callback function which will be executed time to time.
        /// </summary>
        /// <param name="state">not used yet</param>
        private void TimerProc(object state)
        {
            if (Enabled)
            {
                base.RaiseOnTick(this, new EventArgs());
            }
        }
    }
}
