using EventBusRabbitMQ.Events;

namespace LogMessageBroker.API.Interface
{
    public interface ILogExceptionService
    {
        Task AddAsync(string colletionName, LogExceptionsEvent log);
        Task<IEnumerable<LogExceptionsEvent>> GetAllPeopleAsync(string colletionName);
    }
}
