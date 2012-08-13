using System;
using System.Web.Caching;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class Session : ISession
    {
        private static readonly Cache session = new Cache();

        /// <summary>
        /// returns object that associated with key. Returns null if nothing associated
        /// </summary>
        public object Get(string key)
        {
            if (null == key)
            {
                throw new ArgumentNullException();
            }

            return session[key];
        }

        /// <summary>
        /// adds value to session. If the specified key already exists, value will be overwriten.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (null == key || null == value)
            {
                throw new ArgumentNullException();
            }

            session.Add(key, value, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0),
                      CacheItemPriority.NotRemovable, null);
        }

        public T Get<T>(string key) where T : class
        {
            return this.Get(key) as T;
        }
    }
}