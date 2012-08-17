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
        private readonly IRepository repository;

        public InquiryFactory(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Creates Inquiry from MailMessage and save it to repository.
        /// </summary>
        /// <param name="message">Source MailMessage for Inquiry</param>
        public Inquiry CreateInquiry(Entities.MailMessage message)
        {
            var receiver = message.Receivers.First();
            var owner = repository.Query<User>(x=>x.Teams).First(x => x.RelatedPerson.Id == receiver.Id);
            return owner != null ? 
                new Inquiry
                {
                    Client = message.Sender,
                    Description = message.Body,
                    Source = message,
                    Subject = message.Subject,
                    ReferenceDate = null,
                    Owner = owner.CurrentTeam
                }
                :null;
        }


    }
}