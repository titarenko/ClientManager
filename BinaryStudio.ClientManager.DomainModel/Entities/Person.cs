using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Class that represents model of person
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Id of person. It is a primary key
        /// </summary>
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
        /// Date when person was added to system
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Email adress of person
        /// </summary>
        public string Email { get; set; }
    }
}
