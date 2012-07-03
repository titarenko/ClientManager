using System;
using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Class that represents model of person.
    /// </summary>
    public class Person:IIdentifiable
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of person.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of person.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Person's full name.
        /// </summary>
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        /// <summary>
        /// Date when person was added to system.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Email address of person.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Int value that represents Role of person.
        /// For full list of values <see cref="PersonRole"/> enumeration.
        /// </summary>
        public int RoleValue { get; set; }

        /// <summary>
        /// Person role in the system (e.g., client, 
        /// for full list see <see cref="PersonRole"/> enumeration).
        /// It gives and sets role by RoleValue property
        /// </summary>
        public PersonRole Role
        {
            get 
            { 
                return (PersonRole)RoleValue; 
            }

            set
            {
                RoleValue = (int)value;
            }
        }

        /// <summary>
        /// Country where person lives.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Phone number of person.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Photo of the person
        /// </summary>
        public byte[] Photo { get; set; }

        public IList<MailMessage> RelatedMails { get; set; }
        
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="objToCompare">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object objToCompare)
        {
            var personToCompare = (objToCompare as Person);

            return (personToCompare != null &&
                    personToCompare.Id == Id &&
                    personToCompare.CreationDate == CreationDate &&
                    personToCompare.Email == Email &&
                    personToCompare.FirstName == FirstName &&
                    personToCompare.LastName == LastName &&
                    personToCompare.Role == Role);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing 
        /// algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}