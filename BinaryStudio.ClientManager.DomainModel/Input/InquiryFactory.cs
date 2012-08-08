using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class InquiryFactory
    {
        private IRepository repository;

        public InquiryFactory(IRepository repository)
        {
            this.repository = repository;
        }

        public void CreateInquiry(Entities.MailMessage message)
        {
            if (message.Sender.Role == PersonRole.Client &&
                !repository.Query<Inquiry>(x => x.Source)
                .Select(x=>x.Source)
                .Any(message.SameMessagePredicate()))
                
            {
                repository.Save(new Inquiry
                                    {
                                        Client = message.Sender,
                                        Description = message.Body,
                                        Source = message,
                                        Subject = message.Subject,
                                        ReferenceDate = null
                                    });
            }
        }
    }
}