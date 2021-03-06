﻿using System;
using System.Collections.Generic;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class Cache : ICache
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
        /// adds value to cache. If the specified key already exists, value will be overwritten.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (null == key)
            {
                throw new ArgumentNullException();
            }

            lock (session)
            {
                session[key] = value;
            }
        }

        /// <summary>
        /// returns object as T that associated with key. Returns null if nothing associated or object isn't of type T
        /// </summary>
        public T Get<T>(string key) 
        {
            return (T) Get(key) ;
        }
    }
}