using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public static class DateTimeExtensions
    {
        public static DateTime GetStartOfMonth(this DateTime dayWithinMonth)
        {
            return new DateTime(dayWithinMonth.Year, dayWithinMonth.Month, 1);
        }

        public static DateTime GetEndOfMonth(this DateTime dayWithinMonth)
        {
            var firstDayOfTheNextMonth = new DateTime(dayWithinMonth.Year, dayWithinMonth.Month, 1);
            return firstDayOfTheNextMonth.AddMonths(1).AddDays(-1);
        }


        public static DateTime GetStartOfWeek(this DateTime dateWithinWeek, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var difference = dateWithinWeek.DayOfWeek - startOfWeek;
            if (difference < 0)
            {
                difference += 7;
            }
            return dateWithinWeek.AddDays(-difference).Date;
        }

        public static DateTime GetStartOfBusinessWeek(this DateTime dateWithinWeek, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return dateWithinWeek.GetStartOfWeek(startOfWeek);
        }
        
        public static DateTime GetEndOfWeek(this DateTime dateWithinWeek, DayOfWeek endOfWeek = DayOfWeek.Sunday)
        {
            var difference = endOfWeek - dateWithinWeek.DayOfWeek;
            if (difference < 0)
            {
                difference += 7;
            }
            return dateWithinWeek.AddDays(difference).Date;
        }

        public static DateTime GetEndOfBusinessWeek(this DateTime dateWithinWeek, int weekendLength = 2, DayOfWeek endOfWeek = DayOfWeek.Sunday)
        {
            return dateWithinWeek.GetEndOfWeek(endOfWeek).AddDays(-weekendLength);
        }
    }
}