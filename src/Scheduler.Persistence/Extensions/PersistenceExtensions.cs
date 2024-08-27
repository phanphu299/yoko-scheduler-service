using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scheduler.Persistence.Context;
using Scheduler.Application.Repository;
using Scheduler.Persistence.Repository;

namespace Scheduler.Persistence.Extension
{
    public static class PersistenceExtensions
    {
        public static void AddPersistenceService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContextPool<SchedulerDbContext>((service, option) =>
            {
                var configuration = service.GetService<IConfiguration>();
                var connectionString = configuration["ConnectionStrings:Default"];
                option.UseNpgsql(connectionString);
            });
            serviceCollection.AddScoped<ICronExpressionRepository, CronExpressionPersistenceRepository>();
            serviceCollection.AddScoped<IJobRepository, JobPersistenceRepository>();
            serviceCollection.AddScoped<ISchedulerUnitOfWork, SchedulerUnitOfWork>();
            serviceCollection.AddScoped<IJobRepository, JobPersistenceRepository>();
            serviceCollection.AddMemoryCache();
        }
    }
}