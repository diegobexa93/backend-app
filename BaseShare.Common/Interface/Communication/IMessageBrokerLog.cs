using EventBusRabbitMQ.Events;
using Refit;

namespace BaseShare.Common.Interface.Communication
{
    public interface IMessageBrokerLog
    {
        [Post("api/LogExceptions")]
        Task CreateLogUserAPI([Body] LogExceptionsEvent logEvent);
    }
}
