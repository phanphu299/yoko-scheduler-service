using System;
using System.Linq.Expressions;
using Quartz;

namespace Scheduler.Application.Model
{
    public class JobExecutionDto
    {
        public string Key { get; set; }
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }
        public DateTimeOffset? NextFireTimeUtc { get; set; }

        private static Func<ITrigger, JobExecutionDto> Converter = Projection.Compile();
        private static Expression<Func<ITrigger, JobExecutionDto>> Projection
        {
            get
            {
                return model => new JobExecutionDto
                {
                    PreviousFireTimeUtc = model.GetPreviousFireTimeUtc(),
                    NextFireTimeUtc = model.GetNextFireTimeUtc()
                };
            }
        }

        public static new JobExecutionDto Create(ITrigger model)
        {
            if (model == null)
                return null;
            return Converter(model);
        }
    }
}