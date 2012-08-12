namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageParserFactory : IMailMessageParserFactory
    {
        public IMailMessageParser GetMailMessageParser()
        {
            return new MailMessageParserThunderbird();
        }
    }
}
