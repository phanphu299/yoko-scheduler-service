using Scheduler.Application.Service.Abstraction;

namespace Scheduler.Application.Service
{
    public class RequestContext : IRequestContext
    {
        private string _requestBody;
        public string RequestBody => _requestBody;

        public IRequestContext SetBody(string body)
        {
            _requestBody = RequestBody;
            return this;
        }
    }
}