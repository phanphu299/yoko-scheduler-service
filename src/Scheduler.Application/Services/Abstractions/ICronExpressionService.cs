using System;
using Scheduler.Application.Command;
using Scheduler.Application.Model;
using AHI.Infrastructure.Service.Abstraction;

namespace Scheduler.Application.Service.Abstraction
{
    public interface ICronExpressionService : ISearchService<Domain.Entity.CronExpression, Guid, SearchCronExpression, CronExpressionDto>
    {
    }
}