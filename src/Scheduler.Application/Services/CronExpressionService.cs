using System;
using AHI.Infrastructure.Service;
using Scheduler.Application.Command;
using Scheduler.Application.Model;
using Scheduler.Application.Repository;
using Scheduler.Application.Service.Abstraction;

namespace Scheduler.Application.Service
{
    public class CronExpressionService : BaseSearchService<Domain.Entity.CronExpression, Guid, SearchCronExpression, CronExpressionDto>, ICronExpressionService
    {
        public CronExpressionService(IServiceProvider serviceProvider) : base(CronExpressionDto.Create, serviceProvider)
        {
        }

        protected override Type GetDbType()
        {
            return typeof(ICronExpressionRepository);
        }
    }
}