using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public static class StringExtensions
    {
        /// <summary>
        /// Check if string is empty, or is null or is only spaces in it.
        /// </summary>
        public static bool IsNullOrEmpty(this string line)
        {
            return String.IsNullOrEmpty(line);
        }

        /// <summary>
        /// Check if string is empty, or null or only spaces in it then return default value. Otherwise return current string.
        /// </summary>
        /// <param name="defaultValue">Default value</param>
        public static string Or(this string line, string defaultValue)
        {
            return line.IsNullOrEmpty() ? defaultValue : line;
        }

        /// <summary>
        /// Fill string if it is a format string with current arguments.
        /// </summary>
        /// <param name="args">Format args</param>
        /// <returns>Formatted string</returns>
        public static string Fill(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// If string is more then maximum length then it will be cutten for maximum length and last 3 symbols will be changed for "..."
        /// <example>
        /// string s="This is string";
        /// var result=s.Cut(7); //result will be equal "This..."
        /// </example>
        /// </summary>
        /// <param name="maxLength">Maximum length of string</param>
        public static string Cut(this string line, int maxLength)
        {
            return line.SafeGet(x => x.Length) > maxLength ? line.Substring(0, maxLength - 3) + "..." : line;
        }

    }
}