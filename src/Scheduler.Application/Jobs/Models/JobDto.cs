using System;
using System.Linq.Expressions;

namespace Scheduler.Application.Model
{
    public class JobDto
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Cron { get; set; }
        public string PrimaryCron { get; set; }
        public string Type { get; set; }
        public string SchedulerBase { get; set; }
        public string TimeZoneName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }

        private static Func<Domain.Entity.Job, JobDto> Converter = Projection.Compile();
        private static Expression<Func<Domain.Entity.Job, JobDto>> Projection
        {
            get
            {
                return entity => new JobDto
                {
                    Id = entity.Id,
                    Key = entity.Key,
                    Cron = entity.Cron,
                    PrimaryCron = entity.PrimaryCron,
                    Type = entity.Type,
                    SchedulerBase = entity.SchedulerBase,
                    TimeZoneName = entity.TimeZoneName,
                    Start = entity.Start,
                    End = entity.End,
                    CreatedUtc = entity.CreatedUtc,
                    UpdatedUtc = entity.UpdatedUtc
                };
            }
        }

        public static new JobDto Create(Domain.Entity.Job entity)
        {
            if (entity == null)
                return null;
            return Converter(entity);
        }
    }
}