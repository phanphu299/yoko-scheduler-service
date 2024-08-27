using System;
using System.Threading.Tasks;
using AHI.Infrastructure.SharedKernel.Abstraction;
using AHI.Infrastructure.SharedKernel.Extension;
using Scheduler.Application.Helper;
using Scheduler.Application.Command;
using Scheduler.Application.Repository;
using Scheduler.Application.Service.Abstraction;
using Quartz;

namespace Scheduler.Application.Service
{
    public class FutureAddRecurringJob : BaseJob, IJob
    {
        private readonly ILoggerAdapter<IJob> _logger;
        private readonly ISchedulerUnitOfWork _unitOfWork;
        private readonly IJobService _jobService;

        public FutureAddRecurringJob(ILoggerAdapter<IJob> logger, ISchedulerUnitOfWork unitOfWork, IJobService jobService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _jobService = jobService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var executionTimeUtc = DateTime.UtcNow;
            var id = context.JobDetail.Key.Name;

            _logger.LogInformation($"***Delay job {id} runs at {executionTimeUtc.ToString(DateTimeHelper.DATETIME_FORMAT)} UTC");

            try
            {
                var job = await _unitOfWork.JobRepository.FindByIdAsync(id);
                var command = job.RequestJsonContent.FromJson<UpsertRecurringJob>();

                // At this time the function will create a recurring job instead, the delay job will automaticlly deleted by the Quartz
                await _jobService.UpsertRecurringJobAsync(command);
            }
            catch (JobExecutionException ex)
            {
                _logger.LogError($"***Delay job {id} has error");
                throw ex;
            }
        }
    }
}