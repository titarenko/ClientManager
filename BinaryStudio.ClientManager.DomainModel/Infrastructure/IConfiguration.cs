namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    /// <summary>
    /// Provides access to configuration values. Behaves like dictionary.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Returns subsection by name.
        /// </summary>
        /// <returns>
        /// Configuration instance representing requested subsection.
        /// </returns>
        IConfiguration GetSubsection(string name);

        /// <summary>
        /// Returns value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        string GetValue(string key);
        
        /// <summary>
        /// Returns value converted to requested type by key.
        /// </summary>
        /// <typeparam name="T">Type of returned value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Value converted to requested type.</returns>
        T GetValue<T>(string key);
    }
}