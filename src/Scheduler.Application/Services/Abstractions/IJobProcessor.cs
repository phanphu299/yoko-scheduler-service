using System.Threading.Tasks;
using System.Collections.Generic;
using Scheduler.Application.Model;
using Quartz;

namespace Scheduler.Application.Service.Abstraction
{
    /// <summary>
    /// Quartz job business logic
    /// </summary>
    public interface IJobProcessor
    {
        Task<ITrigger> GetTriggerAsync(string key);
        Task AddRecurringJobAsync(JobModel jobModel);
        Task AddDelayJobAsync(JobModel jobModel);
        Task DeleteRecurringJobAsync(string key);
        Task DeleteRecurringJobsAsync(IEnumerable<string> keys);
    }
}