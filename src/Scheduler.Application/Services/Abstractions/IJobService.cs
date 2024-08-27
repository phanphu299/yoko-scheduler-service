using System.Threading.Tasks;
using Scheduler.Application.Command;
using Scheduler.Application.Model;
using AHI.Infrastructure.SharedKernel.Model;

namespace Scheduler.Application.Service.Abstraction
{
    public interface IJobService
    {
        Task<JobExecutionDto> FindJobExecutionByIdAsync(GetJobExecutionById command);
        Task<JobDto> UpsertRecurringJobAsync(UpsertRecurringJob command);
        Task<BaseResponse> DeleteRecurringJobsAsync(DeleteRecurringJob command);
    }
}