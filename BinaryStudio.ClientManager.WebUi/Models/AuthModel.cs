using System.Collections.Generic;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class LogOnModel
    {
        /// <summary>
        /// Collection of log in URIs for all registered providers.
        /// </summary>
        public IEnumerable<string> LoginUris { get; set; }
    }
}