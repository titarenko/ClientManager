namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public interface ISession
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

        /// <summary>
        /// returns object as T that associated with key. Returns null if nothing associated or object isn't of type T
        /// </summary>
        T Get<T>(string key);
    }
}