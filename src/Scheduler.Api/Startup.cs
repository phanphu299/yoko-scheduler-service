using AHI.Infrastructure.Exception.Filter;
using AHI.Infrastructure.MultiTenancy.Extension;
using AHI.Infrastructure.SharedKernel.Extension;
using AHI.Infrastructure.UserContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scheduler.Api.Filter;
using Scheduler.Application.Extension;
using Scheduler.Persistence.Extension;
using Prometheus;
using Prometheus.SystemMetrics;
using Quartz;
using Quartz.AspNetCore;

namespace Scheduler.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationService();
            services.AddPersistenceService();
            services.AddMultiTenantService();

            services.AddControllers(option =>
            {
                option.ExceptionHandling();
            }).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.NullValueHandling = Constant.JsonSerializerSetting.NullValueHandling;
                option.SerializerSettings.DateFormatString = Constant.JsonSerializerSetting.DateFormatString;
                option.SerializerSettings.ReferenceLoopHandling = Constant.JsonSerializerSetting.ReferenceLoopHandling;
                option.SerializerSettings.DateParseHandling = Constant.JsonSerializerSetting.DateParseHandling;
            });

            services.AddAuthentication()
                .AddIdentityServerAuthentication("oidc",
                    jwtTokenOption =>
                    {
                        jwtTokenOption.Authority = Configuration["Authentication:Authority"];
                        jwtTokenOption.RequireHttpsMetadata = Configuration["Authentication:Authority"].StartsWith("https");
                        jwtTokenOption.TokenValidationParameters.ValidateAudience = false;
                        jwtTokenOption.ClaimsIssuer = Configuration["Authentication:Issuer"];
                    }, referenceTokenOption =>
                    {
                        referenceTokenOption.IntrospectionEndpoint = Configuration["Authentication:IntrospectionEndpoint"];
                        referenceTokenOption.ClientId = Configuration["Authentication:ApiScopeName"];
                        referenceTokenOption.ClientSecret = Configuration["Authentication:ApiScopeSecret"];
                        referenceTokenOption.ClaimsIssuer = Configuration["Authentication:Issuer"];
                        referenceTokenOption.SaveToken = true;
                    }
                );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", Configuration["Authentication:ApiScopeName"]);
                });
            });

            /* Begin config Quartz */
            // Base configuration for DI, read from appSettings.json
            services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));

            // If you are using persistent job store, you might want to alter some options
            services.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true; // default: false
                options.Scheduling.OverWriteExistingData = true; // default: true
            });

            services.AddQuartz(q =>
            {
                q.UsePersistentStore(s =>
                {
                    s.UsePostgres(psql =>
                    {
                        psql.ConnectionString = Configuration["ConnectionStrings:Default"];
                    });
                    s.UseNewtonsoftJsonSerializer();
                });
            });

            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            /* End config Quartz */

            services.AddSystemMetrics();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseHttpMetrics();
            app.UseMiddleware<RequestContextMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWhen(
                context => !(context.Request.Path.HasValue && context.Request.Path.Value.StartsWith("/metrics")),
                builder =>
                {
                    builder.UseMiddleware<UserContextMiddleware>();
                });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers()
                       .RequireAuthorization("ApiScope");
            });
        }
    }
}