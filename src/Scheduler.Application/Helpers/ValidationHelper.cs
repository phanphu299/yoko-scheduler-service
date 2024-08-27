using System;
using AHI.Infrastructure.Exception;
using AHI.Infrastructure.Exception.Helper;
using Scheduler.Application.Command;

namespace Scheduler.Application.Helper
{
    public static class ValidationHelper
    {
        public static void ValidateRecurringJob(UpsertRecurringJob command)
        {
            ValidateCronExpression(command.Cron);
            ValidateTimeRange(command.Start, command.End);
            ValidateTimeZone(command.TimeZoneName);
        }

        public static void ValidateCronExpression(string cron)
        {
            if (!CronJobHelper.IsValidCronExpression(cron))
                throw EntityValidationExceptionHelper.GenerateException(nameof(cron), ExceptionErrorCode.DetailCode.ERROR_VALIDATION_INVALID);
        }

        public static void ValidateTimeRange(DateTime? start, DateTime? end)
        {
            if (start != null && end != null && start > end)
                throw EntityValidationExceptionHelper.GenerateException(nameof(start), ExceptionErrorCode.DetailCode.ERROR_VALIDATION_INVALID);
        }

        public static void ValidateTimeZone(string timeZoneName)
        {
            var timeZoneInfo = DateTimeHelper.GetTimeZoneInfo(timeZoneName);
            if (timeZoneInfo == null)
            {
                throw EntityValidationExceptionHelper.GenerateException(nameof(timeZoneName), ExceptionErrorCode.DetailCode.ERROR_VALIDATION_INVALID);
            }
        }
    }
}