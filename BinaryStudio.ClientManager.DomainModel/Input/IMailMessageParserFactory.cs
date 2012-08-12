namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public interface IMailMessageParserFactory
    {
        IMailMessageParser GetMailMessageParser();
    }
}