using System;
using System.Configuration;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// Provides access to configuration values. Behaves like dictionary.
    /// </summary>
    public class AppConfiguration : IConfiguration
    {
        /// <summary>
        /// Returns subsection by name.
        /// </summary>
        /// <param name="name">Subsection name.</param>
        /// <returns>Configuration instance representing requested subsection.</returns>
        public IConfiguration GetSubsection(string name)
        {
            return ConfigurationManager.GetSection(name) as IConfiguration;
        }

        /// <summary>
        /// Returns value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value if key exists. Otherwise null.</returns>
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Returns value converted to requested type by key.
        /// </summary>
        /// <typeparam name="T">Type of returned value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Value converted to requested type.</returns>
        public T GetValue<T>(string key)
        {
            string value = ConfigurationManager.AppSettings[key];

            try
            {
                return (T)Convert.ChangeType(
                    value,
                    typeof(T),
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}