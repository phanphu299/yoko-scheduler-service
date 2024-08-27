using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scheduler.Persistence.Configuration
{
    public class JobConfiguration : IEntityTypeConfiguration<Domain.Entity.Job>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.Job> builder)
        {
            builder.ToTable("jobs");
            builder.HasQueryFilter(x => !x.Deleted);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Key).HasColumnName("key");
            builder.Property(x => x.Cron).HasColumnName("cron");
            builder.Property(x => x.PrimaryCron).HasColumnName("primary_cron");
            builder.Property(x => x.Type).HasColumnName("type");
            builder.Property(x => x.ExecutionJobObject).HasColumnName("execution_job_object");
            builder.Property(x => x.SchedulerBase).HasColumnName("scheduler_base");
            builder.Property(x => x.TimeZoneName).HasColumnName("timezone_name");
            builder.Property(x => x.Start).HasColumnName("start");
            builder.Property(x => x.End).HasColumnName("end");
            builder.Property(x => x.RequestJsonContent).HasColumnName("request_json_content");
            builder.Property(x => x.UpdatedUtc).HasColumnName("updated_utc");
            builder.Property(x => x.CreatedUtc).HasColumnName("created_utc");
            builder.Property(x => x.Deleted).HasColumnName("deleted");
        }
    }
}