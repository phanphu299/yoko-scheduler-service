using System;
using System.Linq.Expressions;

namespace Scheduler.Application.Model
{
    public class CronExpressionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }

        public static Expression<Func<Domain.Entity.CronExpression, CronExpressionDto>> Projection
        {
            get
            {
                return entity => new CronExpressionDto
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Name = entity.Name,
                    Cron = entity.Cron,
                    CreatedUtc = entity.CreatedUtc,
                    UpdatedUtc = entity.UpdatedUtc
                };
            }
        }

        public static CronExpressionDto Create(Domain.Entity.CronExpression entity)
        {
            if (entity == null)
                return null;
            return Projection.Compile().Invoke(entity);
        }
    }
}