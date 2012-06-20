using System;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Tests
{
    /// <summary>
    /// Timer implementation for testing purposes.
    /// </summary>
    class FakeTimer : Timer
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