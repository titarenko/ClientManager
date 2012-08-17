using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Class creates inquiry from MailMessage
    /// </summary>
    public class InquiryFactory : IInquiryFactory
    {
        /// <summary>
        /// Creates Inquiry from MailMessage and save it to repository.
        /// </summary>
        /// <param name="message">Source MailMessage for Inquiry</param>
        public Inquiry CreateInquiry(Entities.MailMessage message)
        {
            return new Inquiry
                    {
                        Client = message.Sender,
                        Description = message.Body,
                        Source = message,
                        Subject = message.Subject,
                        ReferenceDate = null,
                    };
        }


    }
}