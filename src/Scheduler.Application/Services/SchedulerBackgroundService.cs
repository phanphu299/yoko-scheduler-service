using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using AHI.Infrastructure.Exception;
using AHI.Infrastructure.SharedKernel.Abstraction;
using Scheduler.Application.Command;
using Scheduler.Application.Model;
using Scheduler.Application.Helper;
using Scheduler.Application.Service.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Scheduler.Application.Service
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Channel<QueueMessage> _channel;
        private readonly ILoggerAdapter<SchedulerBackgroundService> _logger;

        public SchedulerBackgroundService(IServiceProvider serviceProvider, ILoggerAdapter<SchedulerBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _channel = Channel.CreateUnbounded<QueueMessage>();
            _logger = logger;
        }

        public async Task QueueAsync(object command)
        {
            if (command == null)
            {
                throw new EntityInvalidException(nameof(command));
            }
            await _channel.Writer.WriteAsync(new QueueMessage(command));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var queueMessage = await _channel.Reader.ReadAsync(stoppingToken);
                try
                {
                    switch (queueMessage.Command)
                    {
                        case ExecuteOnetimeJob command:
                            await ExecuteOnetimeJobAsync(command);
                            break;

                        default:
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task ExecuteOnetimeJobAsync(ExecuteOnetimeJob command)
        {
            // Delay 5 sec for the service call flow to be finished
            await Task.Delay(5000);

            var executionTimeUtc = DateTimeHelper.GetUtcDateTime(command.Job.Start, command.Job.TimeZoneName);
            var requestJsonContent = RequestJsonContentDto.Create(command.Job);

            try
            {
                _logger.LogInformation($"***Onetime job {requestJsonContent.Id} runs at {executionTimeUtc.ToString(DateTimeHelper.DATETIME_FORMAT)} UTC");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var httpExecutionService = scope.ServiceProvider.GetService(typeof(IHttpExecutionService)) as IHttpExecutionService;

                    // No need to use await because there is nothing to do with the response
                    _ = httpExecutionService.ExecuteAsync(requestJsonContent, executionTimeUtc);
                }
            }
            catch
            {
                _logger.LogError($"***Onetime job {requestJsonContent.Id} has error");
                throw;
            }
        }

        private class QueueMessage
        {
            public object Command { get; set; }

            public QueueMessage(object command)
            {
                Command = command;
            }
        }
    }
}