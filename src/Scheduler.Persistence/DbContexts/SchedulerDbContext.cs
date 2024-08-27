using Microsoft.EntityFrameworkCore;
using Scheduler.Persistence.Configuration;

namespace Scheduler.Persistence.Context
{
    public class SchedulerDbContext : DbContext
    {
        public DbSet<Domain.Entity.Job> Jobs { get; set; }

        public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CronExpressionConfiguration());
            modelBuilder.ApplyConfiguration(new JobConfiguration());
        }
    }
}