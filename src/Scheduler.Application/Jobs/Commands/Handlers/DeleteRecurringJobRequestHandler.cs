using System.Threading;
using System.Threading.Tasks;
using AHI.Infrastructure.SharedKernel.Model;
using Scheduler.Application.Service.Abstraction;
using MediatR;

namespace Scheduler.Application.Command.Handler
{
    public class DeleteRecurringJobRequestHandler : IRequestHandler<DeleteRecurringJob, BaseResponse>
    {
        private readonly IJobService _service;

        public DeleteRecurringJobRequestHandler(IJobService service)
        {
            _service = service;
        }

        public Task<BaseResponse> Handle(DeleteRecurringJob request, CancellationToken cancellationToken)
        {
            return _service.DeleteRecurringJobsAsync(request);
        }
    }
}