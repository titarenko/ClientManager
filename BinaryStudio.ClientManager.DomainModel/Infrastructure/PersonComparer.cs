using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// Comparer class that compares Persons
    /// </summary>
    public class PersonComparer:IEqualityComparer<Person>
    {
        /// <summary>
        /// Compare 2 objects of person class
        /// </summary>
        /// <param name="person1">1st object</param>
        /// <param name="person2">2nd object</param>
        /// <returns>Return true if they are the same.
        /// Return false if they are not the same.
        /// </returns>
        public bool Equals(Person person1, Person person2)
        {
            if (person1.FirstName == person2.FirstName && person1.CreationDate == person2.CreationDate &&
                person1.Email == person2.Email && person1.Id == person2.Id
                && person1.LastName == person2.LastName)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        /// <summary>
        /// Return Hash Code of the object. Only Id property is used.
        /// </summary>
        /// <param name="person">Object of the Person class</param>
        /// <returns>Hashcode of the object</returns>
        public int GetHashCode(Person person)
        {
            int hCode = person.Id;
            return hCode.GetHashCode();
        }
    }
}
