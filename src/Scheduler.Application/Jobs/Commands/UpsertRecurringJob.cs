using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using AHI.Infrastructure.SharedKernel.Extension;
using Scheduler.Application.Enum;
using Scheduler.Application.Model;
using MediatR;
using Scheduler.Application.Helper;

namespace Scheduler.Application.Command
{
    public class UpsertRecurringJob : IRequest<JobDto>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Cron { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string TimeZoneName { get; set; }
        public SchedulerBase SchedulerBase { get; set; } = SchedulerBase.StartDate;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public IDictionary<string, object> AdditionalParams { get; set; }

        /* Begin additional properties */
        public DateTimeOffset TriggerStart { get; private set; }
        public string Type { get; private set; }
        public string PrimaryCron { get; private set; }
        public string Key { get; private set; }
        public ExecutionJobObject ExecutionJobObject { get; private set; }
        /* End additional properties */

        public void SetTriggerStart(DateTimeOffset startTime)
        {
            TriggerStart = startTime;
        }

        public void SetType(string type)
        {
            Type = type;
        }

        public void SetPrimaryCron(string primaryCron)
        {
            PrimaryCron = primaryCron;
        }

        public void SetKey(string key)
        {
            Key = key;
        }

        public void SetExecutionJobObject(ExecutionJobObject executionJobObject)
        {
            ExecutionJobObject = executionJobObject;
        }

        private static Func<UpsertRecurringJob, Domain.Entity.Job> ConverterEntity = ProjectionEntity.Compile();
        public static Expression<Func<UpsertRecurringJob, Domain.Entity.Job>> ProjectionEntity
        {
            get
            {
                return command => new Domain.Entity.Job
                {
                    Id = command.Id,
                    Key = command.Key,
                    Cron = command.Cron,
                    PrimaryCron = command.PrimaryCron,
                    Type = command.Type,
                    ExecutionJobObject = command.ExecutionJobObject.ToString(),
                    SchedulerBase = command.SchedulerBase.ToString(),
                    TimeZoneName = command.TimeZoneName,
                    Start = command.Start.Value,
                    End = command.End.Value,
                    RequestJsonContent = command.ToJson(),
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };
            }
        }

        public static Domain.Entity.Job CreateEntity(UpsertRecurringJob command)
        {
            if (command == null)
                return null;
            return ConverterEntity(command);
        }

        private static Func<UpsertRecurringJob, JobModel> ConverterModel = ProjectionModel.Compile();
        public static Expression<Func<UpsertRecurringJob, JobModel>> ProjectionModel
        {
            get
            {
                return command => new JobModel
                {
                    Id = command.Id,
                    Key = command.Key,
                    Cron = command.PrimaryCron,
                    Type = command.Type,
                    ExecutionJobObject = command.ExecutionJobObject,
                    SchedulerBase = command.SchedulerBase,
                    TimeZoneName = command.TimeZoneName,
                    Start = command.Start.Value,
                    End = command.End.Value,
                    TriggerStart = command.TriggerStart
                };
            }
        }

        public static JobModel CreateModel(UpsertRecurringJob command)
        {
            if (command == null)
                return null;
            return ConverterModel(command);
        }
    }
}