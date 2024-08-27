using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using AHI.Infrastructure.MultiTenancy.Extension;
using AHI.Infrastructure.SharedKernel.Abstraction;
using Scheduler.Application.Constant;
using Scheduler.Application.Helper;
using Scheduler.Application.Model;
using Scheduler.Application.Service.Abstraction;

namespace Scheduler.Application.Service
{
    public class HttpExecutionService : IHttpExecutionService
    {
        private readonly ILoggerAdapter<IHttpExecutionService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpExecutionService(ILoggerAdapter<IHttpExecutionService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// *NOTE: No need to use await because there is nothing to do with the response
        /// </summary>
        public async Task ExecuteAsync(RequestJsonContentDto content, DateTime executionTimeUtc)
        {
            try
            {
                var requestDataBuilder = new RequestDataBuilder(content);

                requestDataBuilder.SetExecutionTimeUtc(executionTimeUtc);

                var requestData = requestDataBuilder.Build();
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.SERVICE, requestData.TenantContext);

                _logger.LogInformation(@$"***
                                        Now:      {requestData.ExecutionTimeUtc.ToString(DateTimeHelper.DATETIME_FORMAT)} 
                                        Next:     {requestData.NextExecutionTimeUtc?.ToString(DateTimeHelper.DATETIME_FORMAT)} 
                                        Previous: {requestData.PreviousExecutionTimeUtc?.ToString(DateTimeHelper.DATETIME_FORMAT)}"
                                        );

                if (content.Method.ToLower() == HttpRequestMethod.GET)
                    _ = httpClient.GetAsync(requestData.RequestUrl);

                if (content.Method.ToLower() == HttpRequestMethod.POST)
                {
                    _ = httpClient.PostAsync(
                        requestData.RequestUrl,
                        new StringContent(requestData.JsonPayload, Encoding.UTF8, MediaType.APPLICATION_JSON)
                    );
                }
            }
            catch
            {
                _logger.LogError($"***HTTP call with job {content.Id} has error");
                throw;
            }
        }
    }
}