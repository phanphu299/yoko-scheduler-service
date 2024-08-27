using System.Threading.Tasks;
using Scheduler.Application.Command;
using Scheduler.Application.Extensions;
using Scheduler.Application.Helper;
using Scheduler.Application.Model;
using Scheduler.Application.Constant;
using Scheduler.Application.Repository;
using Scheduler.Application.Service.Abstraction;
using AHI.Infrastructure.SharedKernel.Model;

namespace Scheduler.Application.Service
{
    public class JobService : IJobService
    {
        private readonly IJobProcessor _jobProcessor;
        private readonly ISchedulerUnitOfWork _unitOfWork;
        private readonly SchedulerBackgroundService _backgroundService;

        public JobService(IJobProcessor jobProcessor, ISchedulerUnitOfWork unitOfWork, SchedulerBackgroundService backgroundService)
        {
            _jobProcessor = jobProcessor;
            _unitOfWork = unitOfWork;
            _backgroundService = backgroundService;
        }

        public async Task<JobExecutionDto> FindJobExecutionByIdAsync(GetJobExecutionById command)
        {
            var jobEntity = await _unitOfWork.JobRepository.FindByIdAsync(command.Id);
            var triggerEntity = await _jobProcessor.GetTriggerAsync(jobEntity.Key);
            return JobExecutionDto.Create(triggerEntity);
        }

        public async Task<JobDto> UpsertRecurringJobAsync(UpsertRecurringJob command)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                ValidationHelper.ValidateRecurringJob(command);

                command.DecorateTimeRange();
                command.DecorateType();
                command.DecorateCron();
                command.DecorateKey();
                command.DecorateExecutionJobObject();

                var requestJobModel = UpsertRecurringJob.CreateModel(command);
                var requestJobEntity = UpsertRecurringJob.CreateEntity(command);

                if (requestJobModel.Type == JobType.RECURRING)
                    await _jobProcessor.AddRecurringJobAsync(requestJobModel);
                else if (requestJobModel.Type == JobType.DELAY)
                    await _jobProcessor.AddDelayJobAsync(requestJobModel);

                await _unitOfWork.JobRepository.UpsertAsync(requestJobEntity);

                await _unitOfWork.CommitAsync();

                return JobDto.Create(requestJobEntity);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// *NOTE: No need to delete Quartz's jobs, they will be deleted at the time they're run if there are no jobs' references
        /// </summary>
        public async Task<BaseResponse> DeleteRecurringJobsAsync(DeleteRecurringJob command)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.JobRepository.DeleteAsync(command.Ids);
                await _unitOfWork.CommitAsync();
                return BaseResponse.Success;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}