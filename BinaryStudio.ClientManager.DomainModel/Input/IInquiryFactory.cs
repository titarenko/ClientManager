using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// Creates Inquiry from MailMessage
    /// </summary>
    public interface IInquiryFactory
    {
        /// <summary>
        /// Creates Inquiry from MailMessage
        /// </summary>
        /// <param name="message">Source MailMessage for Inquiry</param>
        Inquiry CreateInquiry(Entities.MailMessage message);
    }
}