using System.Collections.Generic;
using FizzWare.NBuilder;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    class RandomItem<T>
    {
        private readonly IList<T> list;
        private readonly IRandomGenerator randomGenerator;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="list"> our collection</param>
        /// <param name="unique">if true then random item will be unique; if false then possible recurrence of items</param>
        public RandomItem(IList<T> list, bool unique = false)
        {
            this.list = list;
            randomGenerator = unique ? new UniqueRandomGenerator() : new RandomGenerator();
        }

        /// <summary>
        /// Returns next random item
        /// </summary>
        /// <returns></returns>
        public T Next()
        {
            return list[randomGenerator.Next(0, list.Count)];
        }
    }
}
