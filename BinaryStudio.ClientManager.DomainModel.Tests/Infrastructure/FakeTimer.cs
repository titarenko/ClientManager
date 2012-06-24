using System;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Infrastructure
{
    /// <summary>
    /// Timer implementation for testing purposes.
    /// </summary>
    public class FakeTimer : Timer
    {
        /// <summary>
        /// Raises OnTick event when called.
        /// </summary>
        public void RaiseOnTick()
        {
            RaiseOnTick(this, new EventArgs());
        }
    }
}