namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface IRandomToken
    {
        string GetRandomToken();

        string GetParametrizedRandomToken(string parameter);
    }
}