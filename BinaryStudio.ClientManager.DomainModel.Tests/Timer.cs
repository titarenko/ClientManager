using System;

namespace BinaryStudio.ClientManager.DomainModel.Tests
{
    public class Timer
    {
        public virtual TimeSpan Interval { get; set; }

        public virtual bool Enabled { get; set; }

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