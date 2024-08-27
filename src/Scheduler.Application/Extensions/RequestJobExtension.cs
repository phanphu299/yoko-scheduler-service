using Scheduler.Application.Command;
using Scheduler.Application.Constant;
using Scheduler.Application.Enum;
using Scheduler.Application.Helper;

namespace Scheduler.Application.Extensions
{
    public static class RequestJobExtension
    {
        /// <summary>
        /// *NOTE: Start & end are just date time themselves, they don't contain any timezone information
        /// </summary>
        public static void DecorateTimeRange(this UpsertRecurringJob command)
        {
            var now = DateTimeHelper.GetCurrentDateTime(command.TimeZoneName);

            command.Start = command.Start != null
                ? DateTimeHelper.GetUnspecifiedKind(command.Start.Value)
                : now;

            command.SetTriggerStart(DateTimeHelper.GetDateTimeOffset(
                command.Start.Value < now ? now : command.Start.Value, command.TimeZoneName));

            command.End = command.End != null
                ? DateTimeHelper.GetUnspecifiedKind(command.End.Value)
                : command.End = DateTimeHelper.GetMaxDateTime();
        }

        public static void DecorateType(this UpsertRecurringJob command)
        {
            command.SetType(JobType.RECURRING);
        }

        /// <summary>
        /// *NOTE: The cron will be modified in case SchedulerBase = StartDate to make it run in right time
        ///     Ex: Create a cron job runs every 30s, and set start time is 00:20
        ///         - With normal cron job (SchedulerBase = Cron), it will run at
        ///             + 00:30 - First time
        ///             + 01:00 - Second time
        ///             + 01:30 - Third time
        ///         - With modified cron job (SchedulerBase = StartDate), it will run at
        ///             + 00:50 - First time
        ///             + 01:20 - Second time
        ///             + 01:50 - Third time
        /// </summary>
        /// <param name="command"></param>
        public static void DecorateCron(this UpsertRecurringJob command)
        {
            var primaryCron = string.Empty;
            if (command.IsStartDateScheduler())
                primaryCron = CronJobHelper.UpdateCronExpressionBasedOnStartTime(command.Cron, command.Start.Value);
            else
                primaryCron = command.Cron;

            command.SetPrimaryCron(primaryCron);
        }

        /// <summary>
        /// Use command Id and cron string for Job Key.
        /// Don't use the same key for multiple jobs as it could cause unexpected misfires
        /// when multiple jobs are scheduled to run at the same time.
        /// </summary>
        public static void DecorateKey(this UpsertRecurringJob command)
        {
            command.SetKey(command.Id);
        }

        /// <summary>
        /// *NOTE: ExecutionJobObject is the job object will be instantiated at the trigger time (Ref: Scheduler.Application/Jobs)
        /// </summary>
        public static void DecorateExecutionJobObject(this UpsertRecurringJob command)
        {
            if (command.IsRecurringJob())
                command.SetExecutionJobObject(ExecutionJobObject.HttpCallServiceJob);

            if (command.IsDelayJob())
                command.SetExecutionJobObject(ExecutionJobObject.FutureAddRecurringJob);
        }

        private static bool IsStartDateScheduler(this UpsertRecurringJob command)
        {
            return command.SchedulerBase == SchedulerBase.StartDate;
        }

        private static bool IsRecurringJob(this UpsertRecurringJob command)
        {
            return command.Type == JobType.RECURRING;
        }

        private static bool IsDelayJob(this UpsertRecurringJob command)
        {
            return command.Type == JobType.DELAY;
        }
    }
}