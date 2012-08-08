using System.Linq;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Class creates inquiry from MailMessage
    /// </summary>
    public class InquiryFactory : IInquiryFactory
    {
        private IRepository repository;

        public InquiryFactory(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Creates Inquiry from MailMessage and save it to repository.
        /// </summary>
        /// <param name="message">Source MailMessage for Inquiry</param>
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