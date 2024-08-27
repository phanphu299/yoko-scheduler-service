using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AHI.Infrastructure.Repository.Generic;
using Scheduler.Domain.Entity;
using Scheduler.Persistence.Context;
using Scheduler.Application.Repository;

namespace Scheduler.Persistence.Repository
{
    public class JobPersistenceRepository : GenericRepository<Domain.Entity.Job, string>, IJobRepository
    {
        private readonly SchedulerDbContext _context;

        public JobPersistenceRepository(SchedulerDbContext context) : base(context)
        {
            _context = context;
        }

        public override IQueryable<Job> AsQueryable()
        {
            return base.AsQueryable();
        }

        public Task<Job> FindByIdAsync(string id)
        {
            return AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Job>> FindByIdsAsync(IEnumerable<string> ids)
        {
            return await AsQueryable().AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entity.Job>> FindByKeyAsync(string key)
        {
            return await AsQueryable().AsNoTracking().Where(x => x.Key == key).ToListAsync();
        }

        public async Task<Job> UpsertAsync(Domain.Entity.Job requestEntity)
        {
            var targetEntity = await AsQueryable().FirstOrDefaultAsync(x => x.Id == requestEntity.Id);
            if (targetEntity == null)
                return await AddAsync(requestEntity);
            else
                Update(requestEntity, targetEntity);
            return requestEntity;
        }

        public async Task DeleteAsync(IEnumerable<string> ids)
        {
            var targetEntities = await _context.Jobs.Where(j => ids.Contains(j.Id)).ToListAsync();
            _context.Jobs.RemoveRange(targetEntities);
        }

        protected override void Update(Domain.Entity.Job requestObject, Domain.Entity.Job targetObject)
        {
            targetObject.Key = requestObject.Key;
            targetObject.Cron = requestObject.Cron;
            targetObject.PrimaryCron = requestObject.PrimaryCron;
            targetObject.Type = requestObject.Type;
            targetObject.ExecutionJobObject = requestObject.ExecutionJobObject;
            targetObject.SchedulerBase = requestObject.SchedulerBase;
            targetObject.TimeZoneName = requestObject.TimeZoneName;
            targetObject.Start = requestObject.Start;
            targetObject.End = requestObject.End;
            targetObject.RequestJsonContent = requestObject.RequestJsonContent;
            targetObject.CreatedUtc = requestObject.CreatedUtc;
            targetObject.UpdatedUtc = requestObject.UpdatedUtc;
        }
    }
}