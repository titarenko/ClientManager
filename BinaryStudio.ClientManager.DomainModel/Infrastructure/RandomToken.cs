using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class RandomToken : IRandomToken
    {
        public string GetRandomToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
