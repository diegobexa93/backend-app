using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LogMessageBroker.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TraceController(IRabbitMQPublisher<TraceRequestEvent> rabbitMQPublisher) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTraceUserAPI([FromBody] TraceRequestEvent traceEvent)
        {
            await rabbitMQPublisher.PublishMessageAsync(traceEvent, RabbitMQQueues.UserTraceAPI);
            return Ok();
        }
    }
}
