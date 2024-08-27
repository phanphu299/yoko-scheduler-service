using System;
using AHI.Infrastructure.Repository.Generic;

namespace Scheduler.Application.Repository
{
    public interface ICronExpressionRepository : IRepository<Domain.Entity.CronExpression, Guid>
    {
    }
}