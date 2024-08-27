using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Scheduler.Application.Enum;
using Scheduler.Application.Model;
using Scheduler.Application.Constant;
using Scheduler.Application.Service.Abstraction;
using Quartz;
using Scheduler.Application.Helper;

namespace Scheduler.Application.Service
{
    public class JobProcessor : IJobProcessor
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public JobProcessor(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task<ITrigger> GetTriggerAsync(string key)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var triggerKey = new TriggerKey(key, JobGroup.RECURRING_GROUP);
            return await scheduler.GetTrigger(triggerKey);
        }

        /// <summary>
        /// Use recurring job in case the job has start date <= current
        /// *NOTE:
        /// - The recurring trigger & job will be stored in the tables as below, and the records will be reserved after the run
        ///     + qrtz_triggers
        ///     + qrtz_cron_triggers
        ///     + qrtz_job_details
        /// </summary>
        public async Task AddRecurringJobAsync(JobModel jobModel)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var timeZoneInfo = DateTimeHelper.GetTimeZoneInfo(jobModel.TimeZoneName);
            var triggerKey = new TriggerKey(jobModel.Key, JobGroup.RECURRING_GROUP);
            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .StartAt(jobModel.TriggerStart)
                .WithCronSchedule(jobModel.Cron, sc =>
                {
                    sc.InTimeZone(timeZoneInfo);
                    sc.WithMisfireHandlingInstructionIgnoreMisfires();
                })
                .Build();

            if (await scheduler.CheckExists(triggerKey))
            {
                await scheduler.RescheduleJob(triggerKey, trigger);
            }
            else
            {
                var jobKey = new JobKey(jobModel.Key, JobGroup.RECURRING_GROUP);
                var job = GetJobBuilder(jobModel.ExecutionJobObject)
                    .WithIdentity(jobKey)
                    .Build();
                await scheduler.ScheduleJob(job, trigger);
            }
        }

        /// <summary>
        /// Use delay job in case the job has start date > current, the delay trigger only runs once, then creates a recurring trigger (based on cron)
        /// *NOTE:
        /// - The delay trigger & job will be stored in the tables as below, and the records will be deleted after the run
        ///     + qrtz_triggers
        ///     + qrtz_simple_triggers
        ///     + qrtz_job_details
        /// - In case the server was down at the time the deday job was supposed to run, then when the server is up again, the delay job will be run
        /// </summary>
        public async Task AddDelayJobAsync(JobModel jobModel)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var triggerKey = new TriggerKey(jobModel.Key, JobGroup.DELAY_GROUP);
            var jobKey = new JobKey(jobModel.Key, JobGroup.DELAY_GROUP);

            if (await scheduler.CheckExists(jobKey))
                return;

            var startOffset = DateTimeHelper.GetDateTimeOffset(jobModel.Start, jobModel.TimeZoneName);
            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .StartAt(startOffset)
                .WithSimpleSchedule(sb => sb.WithMisfireHandlingInstructionFireNow());

            var trigger = triggerBuilder.Build();
            var jobBuilder = GetJobBuilder(jobModel.ExecutionJobObject);
            var job = jobBuilder.WithIdentity(jobKey).Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public Task DeleteRecurringJobAsync(string key)
        {
            var keys = new List<string> { key };
            return DeleteRecurringJobsAsync(keys);
        }

        /// <summary>
        /// *NOTE: By default, the trigger will be automatically deleted if there are no job references
        /// </summary>
        public async Task DeleteRecurringJobsAsync(IEnumerable<string> keys)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = keys.Select(k => new JobKey(k, JobGroup.RECURRING_GROUP)).ToList();
            await scheduler.DeleteJobs(jobKeys);
        }

        private JobBuilder GetJobBuilder(ExecutionJobObject executionJobObject)
        {
            return executionJobObject switch
            {
                ExecutionJobObject.FutureAddRecurringJob => JobBuilder.Create<FutureAddRecurringJob>(),
                ExecutionJobObject.HttpCallServiceJob => JobBuilder.Create<HttpCallServiceJob>(),
                _ => throw new Exception($"Invalid {nameof(executionJobObject)}")
            };
        }
    }
}