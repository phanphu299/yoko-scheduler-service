namespace Scheduler.Application.Service.Abstraction
{
    public interface IRequestContext
    {
        string RequestBody { get; }
        IRequestContext SetBody(string body);
    }
}