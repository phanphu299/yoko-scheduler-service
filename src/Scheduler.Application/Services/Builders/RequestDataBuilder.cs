using System;
using System.Web;
using System.Collections.Generic;
using AHI.Infrastructure.MultiTenancy.Extension;
using AHI.Infrastructure.MultiTenancy.Internal;
using AHI.Infrastructure.SharedKernel.Extension;
using Scheduler.Application.Constant;
using Scheduler.Application.Extension;
using Scheduler.Application.Helper;
using Scheduler.Application.Model;

namespace Scheduler.Application.Service
{
    public class RequestDataBuilder
    {
        private RequestJsonContentDto _requestContent;
        private RequestData _requestData;

        public RequestDataBuilder(RequestJsonContentDto requestContent)
        {
            _requestContent = requestContent;
            _requestData = new RequestData();
        }

        public void SetExecutionTimeUtc(DateTime executionTimeUtc)
        {
            _requestData.SetExecutionTimeUtc(executionTimeUtc);
        }

        public RequestData Build()
        {
            BuildTenantContext();
            BuildExecutionCron();
            BuildExecutionTime();
            BuildQueryString();
            BuildJsonPayload();
            BuildRequestUrl();

            return _requestData;
        }

        public void BuildTenantContext()
        {
            var tenantContext = new TenantContext();

            tenantContext.RetrieveFromString(_requestContent.AdditionalParams.TenantId, _requestContent.AdditionalParams.SubscriptionId, _requestContent.AdditionalParams.ProjectId);

            _requestData.SetTenantContext(tenantContext);
        }

        public void BuildExecutionCron()
        {
            var executionCron = string.Empty;
            if (_requestContent.IsStartDateScheduler() && _requestContent.IsRunAtSecond())
            {
                var executionTimeLocal = DateTimeHelper.GetLocalDateTime(_requestData.ExecutionTimeUtc, _requestContent.TimeZoneName);
                executionCron = CronJobHelper.UpdateCronExpressionBasedOnStartTime(_requestContent.Cron, executionTimeLocal);
            }
            else
                executionCron = _requestContent.PrimaryCron;

            _requestData.SetExecutionCron(executionCron);
        }

        public void BuildExecutionTime()
        {
            var executionTimeLocal = CronJobHelper.UpdateExecutionTimeFromSpecificTimeCron(_requestData.ExecutionCron, DateTimeHelper.GetLocalDateTime(_requestData.ExecutionTimeUtc, _requestContent.TimeZoneName));
            var executionTimeUtc = DateTimeHelper.GetUtcDateTime(executionTimeLocal, _requestContent.TimeZoneName);
            var executionTimeUnix = DateTimeHelper.GetUnixTimeMilliseconds(executionTimeUtc);

            _requestData.SetExecutionTimeLocal(executionTimeLocal);
            _requestData.SetExecutionTimeUtc(executionTimeUtc);
            _requestData.SetExecutionTimeUnix(executionTimeUnix);

            if (_requestContent.IsStartDateScheduler())
            {
                var nextExecutionTimeLocal = CronJobHelper.GetNextExecution(_requestData.ExecutionCron, executionTimeLocal);
                var previousExecutionTimeLocal = CronJobHelper.GetPreviousExecution(_requestData.ExecutionCron, executionTimeLocal) ?? DateTime.MinValue;

                var nextExecutionTimeUtc = DateTimeHelper.GetUtcDateTime(nextExecutionTimeLocal, _requestContent.TimeZoneName);
                var previousExecutionTimeUtc = DateTimeHelper.GetUtcDateTime(previousExecutionTimeLocal, _requestContent.TimeZoneName);

                var nextExecutionTimeUnix = DateTimeHelper.GetUnixTimeMilliseconds(nextExecutionTimeUtc);
                var peviousExecutionTimeUnix = DateTimeHelper.GetUnixTimeMilliseconds(previousExecutionTimeUtc);

                _requestData.SetNextExecutionTimeLocal(nextExecutionTimeLocal);
                _requestData.SetNextExecutionTimeUtc(nextExecutionTimeUtc);
                _requestData.SetNextExecutionTimeUnix(nextExecutionTimeUnix);
                _requestData.SetPreviousExecutionTimeLocal(previousExecutionTimeLocal);
                _requestData.SetPreviousExecutionTimeUtc(previousExecutionTimeUtc);
                _requestData.SetPreviousExecutionTimeUnix(peviousExecutionTimeUnix);
            }
        }

        public void BuildQueryString()
        {
            var queryStringKeyValues = new Dictionary<string, object>()
            {
                [FieldName.JOB_ID] = _requestContent.Id,
                [FieldName.TIMEZONE_NAME] = _requestContent.TimeZoneName,
                [FieldName.EXECUTION_TIME] = _requestData.ExecutionTimeUnix
            };

            if (_requestContent.IsStartDateScheduler())
            {
                queryStringKeyValues[FieldName.NEXT_EXECUTION_TIME] = _requestData.NextExecutionTimeUnix;
                queryStringKeyValues[FieldName.PREVIOUS_EXECUTION_TIME] = _requestData.PreviousExecutionTimeUnix;
            }

            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            foreach (var queryString in queryStringKeyValues)
            {
                if (!_requestContent.Endpoint.Contains(queryString.Key, StringComparison.OrdinalIgnoreCase))
                    queryBuilder.Add(queryString.Key, queryString.Value.ToString());
            }

            _requestData.SetQueryString(queryBuilder.ToString());
        }

        public void BuildJsonPayload()
        {
            var jsonPayload = _requestContent.AdditionalParams.ToJson();
            _requestData.SetJsonPayload(jsonPayload);
        }

        public void BuildRequestUrl()
        {
            var query = !_requestContent.Endpoint.Contains("?") ? $"?{_requestData.QueryString}" : $"&{_requestData.QueryString}";
            _requestData.SetRequestUrl($"{_requestContent.Endpoint}{query}");
        }
    }
}