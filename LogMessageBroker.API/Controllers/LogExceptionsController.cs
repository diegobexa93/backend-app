using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LogMessageBroker.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogExceptionsController(IRabbitMQPublisher<LogExceptionsEvent> rabbitMQPublisher) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateLogUserAPI([FromBody] LogExceptionsEvent logEvent)
        {
            await rabbitMQPublisher.PublishMessageAsync(logEvent, RabbitMQQueues.UserLogAPI);
            return Ok();
        }
    }
}
