using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AHI.Infrastructure.OpenTelemetry;
using AHI.Infrastructure.Service.Extension;
using AHI.Infrastructure.UserContext.Extension;
using AHI.Infrastructure.Cache.Redis.Extension;
using AHI.Infrastructure.Bus.ServiceBus.Extension;
using AHI.Infrastructure.MultiTenancy.Http.Handler;
using Scheduler.Pipeline;
using Scheduler.Application.Command;
using Scheduler.Application.Constant;
using Scheduler.Application.Validation;
using Scheduler.Application.Service;
using Scheduler.Application.Service.Abstraction;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using Prometheus;
using MediatR;

namespace Scheduler.Application.Extension
{
    public static class ApplicationExtension
    {
        const string SERVICE_NAME = "scheduler-service";
        public static void AddApplicationService(this IServiceCollection serviceCollection)
        {
            /* DI framework */
            serviceCollection.AddApplicationValidator();
            serviceCollection.AddFrameworkServices();
            serviceCollection.AddUserContextService();
            serviceCollection.AddRedisCache();
            serviceCollection.AddRabbitMQ(SERVICE_NAME);
            serviceCollection.AddMediatR(typeof(ApplicationExtension).GetTypeInfo().Assembly);
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            serviceCollection.AddScoped<IRequestContext, RequestContext>();

            /* DI internal */
            serviceCollection.AddScoped<IJobService, JobService>();
            serviceCollection.AddScoped<IJobProcessor, JobProcessor>();
            serviceCollection.AddScoped<ICronExpressionService, CronExpressionService>();
            serviceCollection.AddScoped<IHttpExecutionService, HttpExecutionService>();
            serviceCollection.AddSingleton<SchedulerBackgroundService>();
            serviceCollection.AddHostedService(sp => sp.GetRequiredService<SchedulerBackgroundService>());

            /* HTTP */
            serviceCollection.AddHttpClient(HttpClientName.SERVICE).AddHttpMessageHandler<ClientCrendetialAuthentication>().UseHttpClientMetrics();

            serviceCollection.AddOtelTracingService(SERVICE_NAME, typeof(ApplicationExtension).Assembly.GetName().Version.ToString());

            // For production, no need to output to console, will adapt with open telemetry collector in the future
            serviceCollection.AddLogging(builder =>
            {
                builder.AddOpenTelemetry(option =>
                {
                    option.SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddService(SERVICE_NAME, typeof(ApplicationExtension).Assembly.GetName().Version.ToString())
                    );
                    option.AddOtlpExporter(oltp =>
                    {
                        oltp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                    });
                });
            });
        }

        public static void AddApplicationValidator(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<FluentValidation.IValidator<UpsertRecurringJob>, UpsertRecurringJobValidation>();
            serviceCollection.AddSingleton<FluentValidation.IValidator<DeleteRecurringJob>, DeleteRecurringJobValidation>();
        }
    }
}