using System;
using System.ComponentModel.DataAnnotations;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Class that represents model of person
    /// </summary>
    public class Person:IIdentifiable
    {
        /// <summary>
        /// Id of person. It is a primary key
        /// </summary>
        [Key()]
        public int Id { get; set; }

        /// <summary>
        /// First name of person
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of person
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Person's full name
        /// </summary>
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        /// <summary>
        /// Date when person was added to system
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Email adress of person
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Person role in the system (e.g., client, 
        /// for full list see <see cref="PersonRole"/> enumeration).
        /// </summary>
        public PersonRole Role { get; set; }

        /// <summary>
        /// Equals method that compare 2 object of Person class
        /// </summary>
        /// <param name="objToCompare">Object that will be compared</param>
        /// <returns>Return true if they are equal and 
        /// Return false if they are not equal</returns>
        public override bool Equals(object objToCompare)
        {
            var personToCompare = (objToCompare as Person);
            if (personToCompare != null)
            {
                if (personToCompare.Id==Id && personToCompare.CreationDate==CreationDate 
                    && personToCompare.Email==Email && personToCompare.FirstName==FirstName
                    && personToCompare.LastName==LastName && personToCompare.Role==Role)
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// Return hashcode of the Person class object. Used Id property only
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return Id;
        }
    }
}