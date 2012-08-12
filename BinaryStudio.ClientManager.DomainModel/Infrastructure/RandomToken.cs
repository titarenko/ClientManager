using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class RandomToken : IRandomToken
    {
        //private const string Separator = "##";

        public string GetRandomToken()
        {
            return Guid.NewGuid().ToString();
        }

        //public string GetParametrizedRandomToken(string parameter)
        //{
        //    return parameter + Separator + this.GetRandomToken();
        //}
    }
}
