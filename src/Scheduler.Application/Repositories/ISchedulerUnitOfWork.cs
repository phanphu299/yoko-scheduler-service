using AHI.Infrastructure.Repository.Generic;

namespace Scheduler.Application.Repository
{
    public interface ISchedulerUnitOfWork : IUnitOfWork
    {
        // public ICronExpressionRepository CronExpressionRepository { get; }
        public IJobRepository JobRepository { get; }
    }
}