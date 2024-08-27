using System;
using Scheduler.Persistence.Context;
using Scheduler.Application.Repository;
using AHI.Infrastructure.Repository.Generic;

namespace Scheduler.Persistence.Repository
{
    public class CronExpressionPersistenceRepository : GenericRepository<Domain.Entity.CronExpression, Guid>, ICronExpressionRepository
    {
        public CronExpressionPersistenceRepository(SchedulerDbContext context) : base(context)
        {
        }

        protected override void Update(Domain.Entity.CronExpression requestObject, Domain.Entity.CronExpression targetObject) { }
    }
}