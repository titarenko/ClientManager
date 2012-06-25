using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Details about both inquiry
    /// </summary>
    public class Inquiry : IIdentifiable<int>
    {
        public Inquiry(Person person, MailMessage mailMessage, int id)
        {
            this.Issuer = person;
            this.Issue = mailMessage;
            this.Id = id;
        }

        /// <summary>
        /// Author of inquiry
        /// </summary>
        public Person Issuer { get; set; }

        /// <summary>
        /// Contains inquiry and subject
        /// </summary>
        public MailMessage Issue { get; set; }

        /// <summary>
        /// implementation of the IIdentifiable for sorting
        /// should return Issue ID
        /// </summary>
        public int Id { get; set; }
    }
}
