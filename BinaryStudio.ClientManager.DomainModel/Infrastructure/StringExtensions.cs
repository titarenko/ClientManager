using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string line)
        {
            return String.IsNullOrWhiteSpace(line);
        }

        public static string Or(this string line, string defaultValue)
        {
            return line.IsNullOrEmpty() ? defaultValue : line;
        }
    }
}