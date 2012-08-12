namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    interface ISession
    {
        /// <summary>
        /// returns object that associated with key. if nothing associated returns null
        /// </summary>
        object Get(string key);

        /// <summary>
        /// adds value to session. If the specified key already exists, value will be overwrites.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, object value);

        T Get<T>(string key) where T : class;
    }
}