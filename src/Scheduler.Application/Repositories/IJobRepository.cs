using System;
using System.Linq;
using System.Threading.Tasks;
using AHI.Infrastructure.Repository.Generic;
using System.Collections.Generic;

namespace Scheduler.Application.Repository
{
    /// <summary>
    /// Internal job business logic
    /// </summary>
    public interface IJobRepository : IRepository<Domain.Entity.Job, string>
    {
        Task<Domain.Entity.Job> FindByIdAsync(string id);
        Task<IEnumerable<Domain.Entity.Job>> FindByIdsAsync(IEnumerable<string> ids);
        Task<IEnumerable<Domain.Entity.Job>> FindByKeyAsync(string key);
        Task<Domain.Entity.Job> UpsertAsync(Domain.Entity.Job requestEntity);
        Task DeleteAsync(IEnumerable<string> ids);
    }
}