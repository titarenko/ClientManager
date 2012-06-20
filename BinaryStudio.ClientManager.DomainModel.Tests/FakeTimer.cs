using System;

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