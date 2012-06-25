using System;
using System.Configuration;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// Provides access to configuration values. Behaves like dictionary.
    /// </summary>
    public class AppConfiguration : IConfiguration
    {
        private readonly string prefix;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefix">Prefix.</param>
        public AppConfiguration(string prefix)
        {
            this.prefix = prefix;
        }

        /// <summary>
        /// Returns subsection by name.
        /// </summary>
        /// <param name="name">Subsection name.</param>
        /// <returns>Configuration instance representing requested subsection.</returns>
        public IConfiguration GetSubsection(string name)
        {
            return new AppConfiguration(name);
        }

        /// <summary>
        /// Returns value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value if key exists. Otherwise null.</returns>
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[String.Format("{0}.{1}", prefix, key)];
        }

        /// <summary>
        /// Returns value converted to requested type by key.
        /// </summary>
        /// <typeparam name="T">Type of returned value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Value converted to requested type.</returns>
        public T GetValue<T>(string key)
        {
            string value = GetValue(key);

            return (T)Convert.ChangeType(
                value,
                typeof(T),
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}