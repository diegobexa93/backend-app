using EventBusRabbitMQ.Events;

namespace LogMessageBroker.API.Interface
{
    public interface ITraceService
    {
        Task AddAsync(string colletionName, TraceRequestEvent trace);
        Task<IEnumerable<TraceRequestEvent>> GetAllAsync(string colletionName);
    }
}
