using Scheduler.Application.Model;
using MediatR;

namespace Scheduler.Application.Command
{
    public class GetJobExecutionById : IRequest<JobExecutionDto>
    {
        public string Id { get; set; }

        public GetJobExecutionById(string id)
        {
            Id = id;
        }
    }
}