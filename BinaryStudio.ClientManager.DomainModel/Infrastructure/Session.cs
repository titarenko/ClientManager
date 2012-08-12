using System;
using System.Collections.Generic;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    // TODO think about implementing thread safety
    public class Session : ISession
    {
        private static readonly Dictionary<string, object> session = new Dictionary<string, object>();

        /// <summary>
        /// returns object that associated with key. Returns null if nothing associated
        /// </summary>
        public object Get(string key)
        {
            if (null == key)
            {
                throw new ArgumentNullException();
            }

            object expectedObject;
            session.TryGetValue(key, out expectedObject);
            return expectedObject;
        }

        /// <summary>
        /// adds value to session. If the specified key already exists, value will be overwriten.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (null == key)
            {
                throw new ArgumentNullException();
            }

            session[key] = value;
        }

        public T Get<T>(string key) where T : class
        {
            return this.Get(key) as T;
        }
    }
}