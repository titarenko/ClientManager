using System;
using System.Globalization;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates date of the first day of month
        /// </summary>
        public static DateTime GetStartOfMonth(this DateTime dayWithinMonth)
        {
            return new DateTime(dayWithinMonth.Year, dayWithinMonth.Month, 1);
        }

        /// <summary>
        /// Calculates date of last day of month
        /// </summary>
        public static DateTime GetEndOfMonth(this DateTime dayWithinMonth)
        {
            var firstDayOfTheNextMonth = new DateTime(dayWithinMonth.Year, dayWithinMonth.Month, 1);
            return firstDayOfTheNextMonth.AddMonths(1).AddDays(-1);
        }


        /// <summary>
        /// Calculates date of the first day of week
        /// </summary>
        public static DateTime GetStartOfWeek(this DateTime dateWithinWeek, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var difference = dateWithinWeek.DayOfWeek - startOfWeek;
            if (difference < 0)
            {
                difference += 7;
            }
            return dateWithinWeek.AddDays(-difference).Date;
        }

        /// <summary>
        /// Calculates date of the first workday within the week
        /// </summary>
        public static DateTime GetStartOfBusinessWeek(this DateTime dateWithinWeek, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return dateWithinWeek.GetStartOfWeek(startOfWeek);
        }

        /// <summary>
        /// Calculates date of last day of week
        /// </summary>
        public static DateTime GetEndOfWeek(this DateTime dateWithinWeek, DayOfWeek endOfWeek = DayOfWeek.Sunday)
        {
            var difference = endOfWeek - dateWithinWeek.DayOfWeek;
            if (difference < 0)
            {
                difference += 7;
            }
            return dateWithinWeek.AddDays(difference).Date;
        }

        /// <summary>
        /// Calculates date of last workday within the week
        /// </summary>
        public static DateTime GetEndOfBusinessWeek(this DateTime dateWithinWeek, int weekendLength = 2, DayOfWeek endOfWeek = DayOfWeek.Sunday)
        {
            return dateWithinWeek.GetEndOfWeek(endOfWeek).AddDays(-weekendLength);
        }

        /// <summary>
        /// Calculates week number
        /// </summary>
        public static int GetWeekNumber (this DateTime dateWithinWeek)
        {
            return CultureInfo
                .CurrentCulture
                .Calendar
                .GetWeekOfYear(dateWithinWeek, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// Defines if specified date is weekend
        /// </summary>
        public static bool IsWeekend(this DateTime date, int weekendLength = 2, DayOfWeek endOfWeek = DayOfWeek.Sunday)
        {
            return date.GetEndOfWeek(endOfWeek).AddDays(-weekendLength).Day < date.Day;
        }
    }
}