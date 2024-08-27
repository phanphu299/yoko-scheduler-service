using AHI.Infrastructure.SharedKernel.Model;
using Scheduler.Application.Constant;
using Scheduler.Application.Model;
using MediatR;

namespace Scheduler.Application.Command
{
    public class SearchCronExpression : BaseCriteria, IRequest<BaseSearchResponse<CronExpressionDto>>
    {
        public bool ClientOverride { get; set; } = false;

        public SearchCronExpression()
        {
            PageSize = 20;
            PageIndex = 0;
            Sorts = DefaultSearch.CUSTOM_SORT;
        }
    }
}