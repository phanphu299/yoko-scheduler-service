using Scheduler.Application.Enum;
using Scheduler.Application.Helper;
using Scheduler.Application.Model;

namespace Scheduler.Application.Extension
{
    public static class RequestJsonContentExtension
    {
        public static bool IsStartDateScheduler(this RequestJsonContentDto content)
        {
            return content.SchedulerBase == SchedulerBase.StartDate;
        }

        public static bool IsCronScheduler(this RequestJsonContentDto content)
        {
            return content.SchedulerBase == SchedulerBase.Cron;
        }

        public static bool IsRunAtSecond(this RequestJsonContentDto content)
        {
            return CronJobHelper.HasOptionEverySecond(content.Cron);
        }
    }
}