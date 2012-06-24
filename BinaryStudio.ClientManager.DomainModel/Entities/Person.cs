using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Email { get; set; }
    }
}
