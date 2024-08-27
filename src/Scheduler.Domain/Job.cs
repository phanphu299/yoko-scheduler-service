using System;
using AHI.Infrastructure.Repository.Model.Generic;

namespace Scheduler.Domain.Entity
{
    public class Job : IEntity<string>
    {
        public string Id { get; set; }

        public string Key { get; set; }

        public string Cron { get; set; }

        public string PrimaryCron { get; set; }

        public string Type { get; set; }

        public string ExecutionJobObject { get; set; }

        public string SchedulerBase { get; set; }

        public string TimeZoneName { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string RequestJsonContent { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime UpdatedUtc { get; set; }

        public bool Deleted { get; set; }
    }
}