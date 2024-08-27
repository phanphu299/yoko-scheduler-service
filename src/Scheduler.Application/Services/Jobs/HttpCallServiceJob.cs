using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AHI.Infrastructure.SharedKernel.Abstraction;
using Scheduler.Application.Model;
using Scheduler.Application.Helper;
using Scheduler.Application.Repository;
using Scheduler.Application.Service.Abstraction;
using Quartz;

namespace Scheduler.Application.Service
{
    public class HttpCallServiceJob : BaseJob, IJob
    {
        private readonly ILoggerAdapter<IJob> _logger;
        private readonly ISchedulerUnitOfWork _unitOfWork;
        private readonly IJobProcessor _jobProcessor;
        private readonly IHttpExecutionService _httpExecutionService;

        public HttpCallServiceJob(ILoggerAdapter<IJob> logger, ISchedulerUnitOfWork unitOfWork, IJobProcessor jobProcessor, IHttpExecutionService httpExecutionService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _jobProcessor = jobProcessor;
            _httpExecutionService = httpExecutionService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var executionTimeUtc = context.ScheduledFireTimeUtc?.UtcDateTime ?? DateTime.UtcNow;
            var key = context.JobDetail.Key.Name;

            _logger.LogInformation($"***Recurring job {key} runs at {executionTimeUtc.ToString(DateTimeHelper.DATETIME_FORMAT)} UTC");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var jobs = await _unitOfWork.JobRepository.FindByKeyAsync(key);

                // Delete Quartz's job if there are no jobs references
                if (!jobs.Any())
                {
                    await _jobProcessor.DeleteRecurringJobAsync(key);
                    return;
                }

                var jobsToRun = jobs.Where(j => DateTimeHelper.HasValidEndRange(j.End, j.TimeZoneName));
                var jobsToDelete = jobs.Except(jobsToRun);

                await RunJobsAsync(executionTimeUtc, jobsToRun);
                await DeleteJobsAsync(jobsToDelete);

                await _unitOfWork.CommitAsync();
            }
            catch (JobExecutionException ex)
            {
                _logger.LogError($"***Recurring job {key} has error");
                throw ex;
            }
        }

        private async Task RunJobsAsync(DateTime executionTime, IEnumerable<Domain.Entity.Job> jobs)
        {
            var tasks = jobs.Select(j => RequestJsonContentDto.Create(j)).Select(j =>
                                RetryHelper.Execute(() => _httpExecutionService.ExecuteAsync(j, executionTime), 3)
                            );
            await Task.WhenAll(tasks);
        }

        private async Task DeleteJobsAsync(IEnumerable<Domain.Entity.Job> jobs)
        {
            await _unitOfWork.JobRepository.DeleteAsync(jobs.Select(j => j.Id));
        }
    }
}