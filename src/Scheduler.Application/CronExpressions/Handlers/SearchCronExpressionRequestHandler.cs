using System.Threading;
using System.Threading.Tasks;
using Scheduler.Application.Model;
using Scheduler.Application.Service.Abstraction;
using AHI.Infrastructure.SharedKernel.Model;
using MediatR;

namespace Scheduler.Application.Command.Handler
{
    public class SearchCronExpressionRequestHandler : IRequestHandler<SearchCronExpression, BaseSearchResponse<CronExpressionDto>>
    {
        private readonly ICronExpressionService _service;

        public SearchCronExpressionRequestHandler(ICronExpressionService service)
        {
            _service = service;
        }

        public Task<BaseSearchResponse<CronExpressionDto>> Handle(SearchCronExpression request, CancellationToken cancellationToken)
        {
            return _service.SearchAsync(request);
        }
    }
}