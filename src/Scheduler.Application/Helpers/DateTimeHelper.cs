using System;
using TimeZoneConverter;

namespace Scheduler.Application.Helper
{
    public static class DateTimeHelper
    {
        public static string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static bool HasValidEndRange(DateTime end, string timeZoneName)
        {
            var current = GetCurrentDateTime(timeZoneName);
            return current < GetUnspecifiedKind(end);
        }

        public static DateTime GetUnspecifiedKind(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        }

        public static DateTime GetSpecifiedKind(DateTime dateTime, DateTimeKind kind)
        {
            return DateTime.SpecifyKind(dateTime, kind);
        }

        public static DateTime GetLocalDateTime(DateTime dateTime, string timeZoneName)
        {
            dateTime = GetSpecifiedKind(dateTime, DateTimeKind.Utc);
            var timeZoneInfo = GetTimeZoneInfo(timeZoneName);
            return TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);
        }

        public static DateTime GetUtcDateTime(DateTime dateTime, string timeZoneName)
        {
            var timeZoneInfo = GetTimeZoneInfo(timeZoneName);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZoneInfo);
        }

        public static DateTimeOffset GetDateTimeOffset(DateTime dateTime, string timeZoneName)
        {
            return new DateTimeOffset(dateTime, GetTimeZoneInfo(timeZoneName).GetUtcOffset(dateTime));
        }

        public static long GetUnixTimeMilliseconds(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Get max datetime
        /// </summary>
        /// <returns>Max datetime (Kind=Unspecified)</returns>
        public static DateTime GetMaxDateTime()
        {
            return DateTime.MaxValue;
        }

        /// <summary>
        /// Get current datetime based on the given timezone
        /// </summary>
        /// <returns>Local datetime (Kind=Unspecified)</returns>
        public static DateTime GetCurrentDateTime(string timeZoneName)
        {
            var timeZoneInfo = GetTimeZoneInfo(timeZoneName);
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
        }

        /// <summary>
        /// The function will get timezoneinfo based on timezone of windows system
        /// </summary>
        /// <param name="timeZoneName"></param>
        /// <returns></returns>
        public static TimeZoneInfo GetTimeZoneInfo(string timeZoneName)
        {
            try
            {
                if (!string.IsNullOrEmpty(timeZoneName))
                    return TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                return TimeZoneInfo.Utc;
            }
            // The exception is throw which means given timeZoneName is invalid or the code is running on linux system
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    string linuxTimeZoneName = TZConvert.WindowsToIana(timeZoneName);
                    return TimeZoneInfo.FindSystemTimeZoneById(linuxTimeZoneName);
                }
                catch (InvalidTimeZoneException)
                {
                    // Invalid timeZoneName
                    return null;
                }
            }
        }
    }
}