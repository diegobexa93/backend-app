using EventBusRabbitMQ.Events;
using Refit;

namespace BaseShare.Common.Interface.Communication
{
    public interface IMessageBrokerLog
    {
        [Post("/api/LogExceptions/CreateLogUserAPI")]
        Task CreateLogUserAPI([Body] LogExceptionsEvent logEvent);

        [Post("/api/Trace/CreateTraceUserAPI")]
        Task CreateTraceUserAPI([Body] TraceRequestEvent logTrace);
    }
}
