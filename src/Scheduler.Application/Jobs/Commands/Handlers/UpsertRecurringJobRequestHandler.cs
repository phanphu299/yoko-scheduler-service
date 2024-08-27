using System.Threading;
using System.Threading.Tasks;
using Scheduler.Application.Model;
using Scheduler.Application.Service.Abstraction;
using MediatR;

namespace Scheduler.Application.Command.Handler
{
    public class UpsertRecurringJobRequestHandler : IRequestHandler<UpsertRecurringJob, JobDto>
    {
        private readonly IJobService _service;

        public UpsertRecurringJobRequestHandler(IJobService service)
        {
            _service = service;
        }

        public Task<JobDto> Handle(UpsertRecurringJob request, CancellationToken cancellationToken)
        {
            return _service.UpsertRecurringJobAsync(request);
        }
    }
}