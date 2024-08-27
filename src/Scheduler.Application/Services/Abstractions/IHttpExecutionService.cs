using System;
using System.Threading.Tasks;
using Scheduler.Application.Model;

namespace Scheduler.Application.Service.Abstraction
{
    public interface IHttpExecutionService
    {
        Task ExecuteAsync(RequestJsonContentDto content, DateTime executionTimeUtc);
    }
}