using BaseShare.Common.Repositories.Mongo;
using EventBusRabbitMQ.Events;
using LogMessageBroker.API.Interface;

namespace LogMessageBroker.API.Services
{
    public class LogExceptionService: ILogExceptionService
    {
        private readonly MongoContext<LogExceptionsEvent> _context;

        public LogExceptionService(MongoContext<LogExceptionsEvent> context)
        {
            _context = context;
        }

        public async Task AddAsync(string colletionName, LogExceptionsEvent log)
        {
            await _context.InsertAsync(colletionName, log);
        }

        public async Task<IEnumerable<LogExceptionsEvent>> GetAllPeopleAsync(string colletionName)
        {
            return await _context.GetAllAsync(colletionName);
        }

    }
}
