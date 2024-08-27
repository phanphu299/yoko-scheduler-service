using AHI.Infrastructure.SharedKernel.Extension;
using Scheduler.Application.Enum;

namespace Scheduler.Application.Model
{
    public class RequestJsonContentDto
    {
        public string Id { get; set; }
        public string Cron { get; set; }
        public string PrimaryCron { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string TimeZoneName { get; set; }
        public SchedulerBase SchedulerBase { get; set; }
        public AdditionalParamsDto AdditionalParams { get; set; }

        public static RequestJsonContentDto Create(Domain.Entity.Job entity)
        {
            return entity.RequestJsonContent.FromJson<RequestJsonContentDto>();
        }
    }
}