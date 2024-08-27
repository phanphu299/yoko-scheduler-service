using System.Collections.Generic;
using AHI.Infrastructure.SharedKernel.Model;
using MediatR;

namespace Scheduler.Application.Command
{
    public class DeleteRecurringJob : IRequest<BaseResponse>
    {
        public IEnumerable<string> Ids { get; set; }

        public DeleteRecurringJob(IEnumerable<string> ids)
        {
            Ids = ids;
        }
    }
}