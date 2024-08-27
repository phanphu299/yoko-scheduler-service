using System;
using Scheduler.Application.Helper;

namespace Scheduler.Application.Extension
{
    public static class DateTimeExtension
    {
        public static TimeSpan GetUtcOffset(this DateTime dateTime, string timeZoneName)
        {
            var timeZoneInfo = DateTimeHelper.GetTimeZoneInfo(timeZoneName);
            return timeZoneInfo.GetUtcOffset(dateTime);
        }

        public static DateTime TrimSeconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0, dt.Kind);
        }
    }
}