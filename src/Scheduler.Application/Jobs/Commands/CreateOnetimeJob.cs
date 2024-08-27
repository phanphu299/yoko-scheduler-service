namespace Scheduler.Application.Command
{
    public class ExecuteOnetimeJob
    {
        public Domain.Entity.Job Job { get; set; }

        public ExecuteOnetimeJob(Domain.Entity.Job job)
        {
            Job = job;
        }
    }
}