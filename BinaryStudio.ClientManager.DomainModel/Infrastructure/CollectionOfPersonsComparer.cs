using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    class CollectionOfPersonsComparer:IEqualityComparer<ICollection<Person>>
    {
        public bool Equals(ICollection<Person> first, ICollection<Person> second)
        {
            if (first.Count!=second.Count)
            {
                return false;
            }
            for (int i = 0; i < first.Count;i++ )
            {
                if (!first.ElementAt(i).Equals(second.ElementAt(i)))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(ICollection<Person> collectionOfPersons)
        {
            return collectionOfPersons.GetHashCode();
        }
    }
}
