using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AHI.Infrastructure.OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;

[assembly: FunctionsStartup(typeof(Scheduler.Function.Startup))]
namespace Scheduler.Function
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {
            System.Diagnostics.Activity.DefaultIdFormat = System.Diagnostics.ActivityIdFormat.W3C;
        }

        public const string SERVICE_NAME = "scheduler-function";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOtelTracingService(SERVICE_NAME, typeof(Startup).Assembly.GetName().Version.ToString());
            builder.Services.AddLogging(builder =>
            {
                builder.AddOpenTelemetry(option =>
                {
                    option.SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddService(SERVICE_NAME, typeof(Startup).Assembly.GetName().Version.ToString())
                    );
                    option.AddOtlpExporter(oltp =>
                    {
                        oltp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                    });
                });
            });
        }
    }
}