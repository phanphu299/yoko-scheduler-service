using AHI.Infrastructure.Repository;
using Scheduler.Persistence.Context;
using Scheduler.Application.Repository;

namespace Scheduler.Persistence.Repository
{
    public class SchedulerUnitOfWork : BaseUnitOfWork, ISchedulerUnitOfWork
    {
        public ICronExpressionRepository CronExpressionRepository { get; private set; }
        public IJobRepository JobRepository { get; private set; }

        public SchedulerUnitOfWork(
                SchedulerDbContext context,
                ICronExpressionRepository cronExpressionRepository,
                IJobRepository scheduleRepository
            )
             : base(context)
        {
            CronExpressionRepository = cronExpressionRepository;
            JobRepository = scheduleRepository;
        }
    }
}