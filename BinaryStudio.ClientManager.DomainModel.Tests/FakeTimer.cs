using System;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Tests
{
    class FakeTimer : Timer
    {
        public void RaiseOnTick()
        {
            RaiseOnTick(this, new EventArgs());
        }
    }
}