using System;
using Scheduler.Application.Enum;

namespace Scheduler.Application.Model
{
    public class JobModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Cron { get; set; }
        public string Type { get; set; }
        public ExecutionJobObject ExecutionJobObject { get; set; }
        public SchedulerBase SchedulerBase { get; set; }
        public string TimeZoneName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTimeOffset TriggerStart { get; set; }
    }
}