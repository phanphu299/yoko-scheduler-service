using System.Threading;
using System.Threading.Tasks;
using Scheduler.Application.Model;
using Scheduler.Application.Service.Abstraction;
using MediatR;

namespace Scheduler.Application.Command.Handler
{
    public class GetTriggerRequestHandler : IRequestHandler<GetJobExecutionById, JobExecutionDto>
    {
        private readonly IJobService _service;

        public GetTriggerRequestHandler(IJobService service)
        {
            _service = service;
        }

        public Task<JobExecutionDto> Handle(GetJobExecutionById request, CancellationToken cancellationToken)
        {
            return _service.FindJobExecutionByIdAsync(request);
        }
    }
}