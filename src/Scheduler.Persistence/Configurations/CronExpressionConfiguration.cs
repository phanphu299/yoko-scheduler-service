using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scheduler.Persistence.Configuration
{
    public class CronExpressionConfiguration : IEntityTypeConfiguration<Domain.Entity.CronExpression>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.CronExpression> builder)
        {
            builder.ToTable("cron_expressions");
            builder.HasQueryFilter(x => !x.Deleted);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Code).HasColumnName("code");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Cron).HasColumnName("cron");
            builder.Property(x => x.CreatedUtc).HasColumnName("created_utc");
            builder.Property(x => x.UpdatedUtc).HasColumnName("updated_utc");
            builder.Property(x => x.Deleted).HasColumnName("deleted");
        }
    }
}