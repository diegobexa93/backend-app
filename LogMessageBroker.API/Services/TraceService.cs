using BaseShare.Common.Repositories.Mongo;
using EventBusRabbitMQ.Events;
using LogMessageBroker.API.Interface;

namespace LogMessageBroker.API.Services
{
    public class TraceService : ITraceService
    {
        private readonly MongoContext<TraceRequestEvent> _context;
        public TraceService(MongoContext<TraceRequestEvent> context)
        {
            _context = context;
        }

        public async Task AddAsync(string colletionName, TraceRequestEvent trace)
        {
            await _context.InsertAsync(colletionName, trace);
        }

        public async Task<IEnumerable<TraceRequestEvent>> GetAllAsync(string colletionName)
        {
            return await _context.GetAllAsync(colletionName);
        }

    }
}
