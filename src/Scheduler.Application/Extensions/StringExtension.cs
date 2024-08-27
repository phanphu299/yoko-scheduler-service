using System;
using System.Linq;

namespace Scheduler.Application.Extension
{
    public static class StringExtension
    {
        public static T ConvertToNumber<T>(this string input)
        {
            var validTypes = new[] { typeof(int), typeof(long), typeof(double), typeof(decimal), typeof(int?), typeof(long?), typeof(double?), typeof(decimal?) };
            if (!validTypes.Any(type => type == typeof(T)))
                throw new InvalidCastException();

            if (input == null || (string)input == string.Empty)
                return default(T);

            var type = typeof(T);
            var nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
                return (T)Convert.ChangeType(input, nullableType);
            else
                return (T)Convert.ChangeType(input, type);
        }
    }
}