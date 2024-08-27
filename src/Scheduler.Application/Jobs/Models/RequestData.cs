using System;
using System.Collections.Generic;
using AHI.Infrastructure.MultiTenancy.Abstraction;

namespace Scheduler.Application.Model
{
    public class RequestData
    {
        public string RequestUrl { get; private set; }
        public string ExecutionCron { get; private set; }
        public string QueryString { get; private set; }
        public string JsonPayload { get; set; }
        public ITenantContext TenantContext { get; private set; }
        public DateTime ExecutionTimeLocal { get; private set; }
        public DateTime ExecutionTimeUtc { get; private set; }
        public long ExecutionTimeUnix { get; private set; }
        public DateTime? NextExecutionTimeLocal { get; private set; }
        public DateTime? NextExecutionTimeUtc { get; private set; }
        public long? NextExecutionTimeUnix { get; private set; }
        public DateTime? PreviousExecutionTimeLocal { get; private set; }
        public DateTime? PreviousExecutionTimeUtc { get; private set; }
        public long? PreviousExecutionTimeUnix { get; private set; }

        public void SetQueryString(string queryString)
        {
            QueryString = queryString;
        }

        public void SetJsonPayload(string jsonPayload)
        {
            JsonPayload = jsonPayload;
        }

        public void SetRequestUrl(string requestUrl)
        {
            RequestUrl = requestUrl;
        }

        public void SetTenantContext(ITenantContext tenantContext)
        {
            TenantContext = tenantContext;
        }

        public void SetExecutionCron(string executionCron)
        {
            ExecutionCron = executionCron;
        }

        public void SetExecutionTimeUtc(DateTime executionTimeUtc)
        {
            ExecutionTimeUtc = executionTimeUtc;
        }

        public void SetExecutionTimeLocal(DateTime executionTimeLocal)
        {
            ExecutionTimeLocal = executionTimeLocal;
        }

        public void SetExecutionTimeUnix(long executionTimeUnix)
        {
            ExecutionTimeUnix = executionTimeUnix;
        }

        public void SetNextExecutionTimeUtc(DateTime nextExecutionTimeUtc)
        {
            NextExecutionTimeUtc = nextExecutionTimeUtc;
        }

        public void SetNextExecutionTimeLocal(DateTime nextExecutionTimeLocal)
        {
            NextExecutionTimeLocal = nextExecutionTimeLocal;
        }

        public void SetNextExecutionTimeUnix(long nextExecutionTimeUnix)
        {
            NextExecutionTimeUnix = nextExecutionTimeUnix;
        }

        public void SetPreviousExecutionTimeUtc(DateTime previousExecutionTimeUtc)
        {
            PreviousExecutionTimeUtc = previousExecutionTimeUtc;
        }

        public void SetPreviousExecutionTimeLocal(DateTime previousExecutionTimeLocal)
        {
            PreviousExecutionTimeLocal = previousExecutionTimeLocal;
        }

        public void SetPreviousExecutionTimeUnix(long previousExecutionTimeUnix)
        {
            PreviousExecutionTimeUnix = previousExecutionTimeUnix;
        }
    }
}