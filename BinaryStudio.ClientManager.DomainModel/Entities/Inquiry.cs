using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Details about both inquiry
    /// </summary>
    public class Inquiry : IIdentifiable
    {
        public Inquiry(Person issuer, MailMessage issue, int id)
        {
            this.Issuer = issuer;
            this.Issue = issue;
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
        /// implementation of the IIdentifiable for quiry
        /// should return Issue ID
        /// </summary>
        public int Id { get; set; }
    }
}
