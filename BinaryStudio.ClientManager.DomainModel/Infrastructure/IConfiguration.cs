namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface IConfiguration
    {
        IConfiguration GetSubsection(string name);

        string GetValue(string key);
        T GetValue<T>(string key);
    }
}