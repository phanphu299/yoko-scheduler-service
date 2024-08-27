using System;
using AHI.Infrastructure.Repository.Model.Generic;

namespace Scheduler.Domain.Entity
{
    public class CronExpression : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public bool Deleted { get; set; }
    }
}