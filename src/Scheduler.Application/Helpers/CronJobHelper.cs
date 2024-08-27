using System;
using System.Collections.Generic;
using System.Linq;
using NCrontab.Advanced;
using NCrontab.Advanced.Enumerations;
using Scheduler.Application.Extension;

namespace Scheduler.Application.Helper
{
    public static class CronJobHelper
    {
        private static readonly string CRON_SEPERATOR = " ";

        public static DateTime GetNextExecution(string expression, DateTime startDate)
        {
            expression = GetOverrideCronExpression(expression);

            var cron = CrontabSchedule.Parse(expression, CronStringFormat.WithSeconds);
            return cron.GetNextOccurrence(startDate);
        }

        public static DateTime? GetPreviousExecution(string expression, DateTime startDate)
        {
            expression = GetOverrideCronExpression(expression);

            var schedule = CrontabSchedule.Parse(expression, CronStringFormat.WithSeconds);

            var expressionSplitted = expression.Split(CRON_SEPERATOR);
            var seconds = expressionSplitted[0];
            var minutes = expressionSplitted[1];
            var hours = expressionSplitted[2];
            var days = expressionSplitted[3];
            var months = expressionSplitted[4];
            var dateOfWeek = expressionSplitted[5];

            if (HasOptionEverySecond(expression))
            {
                var secondsSplitted = seconds.Split("/");
                double.TryParse(secondsSplitted[1], out var second);
                return startDate.AddSeconds(-second);
            }

            var minDate = new DateTime();
            var maxDate = new DateTime();
            if (months != "*")
            {
                minDate = startDate.AddYears(-2);
                maxDate = startDate.AddYears(2);
            }
            else if (days != "*")
            {
                minDate = startDate.AddMonths(-2);
                maxDate = startDate.AddMonths(2);
            }
            else if (dateOfWeek != "*")
            {
                minDate = startDate.AddMonths(-14);
                maxDate = startDate.AddMonths(14);
            }
            else if (hours != "*")
            {
                minDate = startDate.AddDays(-2);
                maxDate = startDate.AddDays(2);
            }
            else if (minutes != "*")
            {
                minDate = startDate.AddHours(-2);
                maxDate = startDate.AddHours(2);
            }
            else if (seconds != "*")
            {
                minDate = startDate.AddMinutes(-2);
                maxDate = startDate.AddMinutes(2);
            }

            var occurrences = schedule.GetNextOccurrences(minDate, maxDate).Cast<DateTime?>().Prepend(default).Pairwise((prev, curr) => new { Previous = prev, Current = curr });
            return occurrences
                .OrderByDescending(x => x.Previous)
                .FirstOrDefault(x => x.Previous.HasValue &&
                                     x.Previous.Value < startDate &&
                                     x.Current.Value.TrimSeconds() <= startDate.TrimSeconds())?.Previous;
        }

        public static bool IsValidCronExpression(string expression)
        {
            return Quartz.CronExpression.IsValidExpression(expression);
        }

        public static string UpdateCronExpressionBasedOnStartTime(string expression, DateTime startTime)
        {
            var expressionSplitted = expression.Split(CRON_SEPERATOR);
            var seconds = expressionSplitted[0];
            var secondsSplitted = seconds.Split("/");
            var hasOptionEverySecond = secondsSplitted.Length == 2;
            if (hasOptionEverySecond)
            {
                secondsSplitted[0] = CalculateStartTimeForCronExpression(startTime.Second, int.Parse(secondsSplitted[1])).ToString();
                seconds = string.Join('/', secondsSplitted);
                expressionSplitted[0] = seconds;
            }

            var minutes = expressionSplitted[1];
            var minutesSplitted = minutes.Split("/");
            var hasOptionEveryMinute = minutesSplitted.Length == 2;
            if (hasOptionEveryMinute)
            {
                minutesSplitted[0] = CalculateStartTimeForCronExpression(startTime.Minute, int.Parse(minutesSplitted[1])).ToString();
                minutes = string.Join('/', minutesSplitted);
                expressionSplitted[1] = minutes;

                if (!hasOptionEverySecond)
                {
                    expressionSplitted[0] = startTime.Second.ToString();
                }
            }

            var hours = expressionSplitted[2];
            var hoursSplitted = hours.Split("/");
            var hasOptionEveryHour = hoursSplitted.Length == 2;
            if (hasOptionEveryHour)
            {
                hoursSplitted[0] = CalculateStartTimeForCronExpression(startTime.Hour, int.Parse(hoursSplitted[1])).ToString();
                hours = string.Join('/', hoursSplitted);
                expressionSplitted[2] = hours;

                if (!hasOptionEverySecond)
                {
                    expressionSplitted[0] = startTime.Second.ToString();
                }

                if (!hasOptionEveryMinute)
                {
                    expressionSplitted[1] = startTime.Minute.ToString();
                }
            }

            var days = expressionSplitted[3];
            var daysSplitted = days.Split("/");
            var hasOptionEveryDay = daysSplitted.Length == 2;
            if (hasOptionEveryDay)
            {
                daysSplitted[0] = CalculateStartTimeForCronExpression(startTime.Day, int.Parse(daysSplitted[1])).ToString();
                if (daysSplitted[0] == "0")
                {
                    daysSplitted[0] = daysSplitted[1];
                }

                days = string.Join('/', daysSplitted);
                expressionSplitted[3] = days;
            }

            var months = expressionSplitted[4];
            var monthsSplitted = months.Split("/");
            var hasOptionEveryMonth = monthsSplitted.Length == 2;
            if (hasOptionEveryMonth)
            {
                monthsSplitted[0] = CalculateStartTimeForCronExpression(startTime.Month, int.Parse(monthsSplitted[1])).ToString();
                if (monthsSplitted[0] == "0")
                {
                    monthsSplitted[0] = monthsSplitted[1];
                }

                months = string.Join('/', monthsSplitted);
                expressionSplitted[4] = months;
            }

            return string.Join(' ', expressionSplitted);
        }

        public static DateTime UpdateExecutionTimeFromSpecificTimeCron(string expression, DateTime startTime)
        {
            var expressionSplitted = expression.Split(CRON_SEPERATOR);
            var seconds = expressionSplitted[0];
            var secondsSplitted = seconds.Split("/");
            var hasSpecificSecond = secondsSplitted.Length == 1 && secondsSplitted[0] != "*";
            var specificSecondInCron = startTime.Second;
            if (hasSpecificSecond)
            {
                int.TryParse(secondsSplitted[0], out var secondsValue);
                specificSecondInCron = secondsValue > 0 ? secondsValue : startTime.Second;
            }

            var minutes = expressionSplitted[1];
            var minutesSplitted = minutes.Split("/");
            var hasSpecificMinutes = minutesSplitted.Length == 1 && minutesSplitted[0] != "*";
            var specificMinutesInCron = startTime.Minute;
            if (hasSpecificMinutes)
            {
                int.TryParse(minutesSplitted[0], out var minutesValue);
                specificMinutesInCron = minutesValue > 0 ? minutesValue : startTime.Minute;
            }

            var hours = expressionSplitted[2];
            var hoursSplitted = hours.Split("/");
            var hasSpecificHours = hoursSplitted.Length == 1 && hoursSplitted[0] != "*";
            var specificHoursInCron = startTime.Hour;
            if (hasSpecificHours)
            {
                int.TryParse(hoursSplitted[0], out var hourValue);
                specificHoursInCron = hourValue > 0 ? hourValue : startTime.Hour;
            }

            var days = expressionSplitted[3];
            var daysSplitted = days.Split("/");
            var hasSpecificDay = daysSplitted.Length == 1 && daysSplitted[0] != "*";
            var specificDayInCron = startTime.Day;
            if (hasSpecificDay)
            {
                int.TryParse(daysSplitted[0], out var dayValue);
                specificDayInCron = dayValue > 0 ? dayValue : startTime.Day;
            }

            var months = expressionSplitted[4];
            var monthsSplitted = months.Split("/");
            var hasSpecificMonth = monthsSplitted.Length == 1 && monthsSplitted[0] != "*";
            var specificMonthInCron = startTime.Month;
            if (hasSpecificMonth)
            {
                int.TryParse(monthsSplitted[0], out var monthValue);
                specificMonthInCron = monthValue > 0 ? monthValue : startTime.Month;
            }

            return new DateTime(startTime.Year, specificMonthInCron, specificDayInCron, specificHoursInCron, specificMinutesInCron, specificSecondInCron);
        }

        public static bool HasOptionEverySecond(string expression)
        {
            var expressionSplitted = expression.Split(CRON_SEPERATOR);
            var seconds = expressionSplitted[0];
            var secondsSplitted = seconds.Split("/");
            return secondsSplitted.Length == 2;
        }

        private static int CalculateStartTimeForCronExpression(int startTime, int cronInterval)
        {
            if (startTime < 0 || cronInterval < 0)
            {
                return 0;
            }

            return startTime >= cronInterval ? startTime % cronInterval : startTime;
        }

        /// <summary>
        /// Quartz day of week is not standard, the purpose of the function is to modify portion day of week in the cron
        /// so that the description can be generated properly
        /// *NOTE: Only for generating description purpose
        /// - Quartz:   1-7 (SUN-SAT)
        /// - Standard: 0-6 (SUN-SAT)
        /// </summary>
        private static string GetOverrideCronExpression(string expression)
        {
            var standardDayOfWeekMapping = new Dictionary<string, string>()
            {
                ["1"] = "0",
                ["2"] = "1",
                ["3"] = "2",
                ["4"] = "3",
                ["5"] = "4",
                ["6"] = "5",
                ["7"] = "6"
            };

            var spitExpression = expression.Split(" ");
            var dayOfWeek = spitExpression[5];

            if (!string.IsNullOrEmpty(dayOfWeek))
            {
                var splitDayOfWeek = dayOfWeek.Split(",");

                for (var i = 0; i < splitDayOfWeek.Length; i++)
                {
                    if (standardDayOfWeekMapping.ContainsKey(splitDayOfWeek[i]))
                    {
                        splitDayOfWeek[i] = standardDayOfWeekMapping[(splitDayOfWeek[i])];
                        continue;
                    }
                }

                spitExpression[5] = string.Join(",", splitDayOfWeek);

                expression = string.Join(" ", spitExpression);
            }

            return expression;
        }
    }
}