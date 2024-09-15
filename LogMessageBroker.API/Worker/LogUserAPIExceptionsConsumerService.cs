using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using LogMessageBroker.API.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace LogMessageBroker.API.Worker
{
    public class LogUserAPIExceptionsConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LogUserAPIExceptionsConsumerService> _logger;
        private readonly RabbitMQSetting _rabbitMqSetting;
        private IConnection _connection;
        private IModel _channel;

        public LogUserAPIExceptionsConsumerService(IOptions<RabbitMQSetting> rabbitMqSetting, 
                                                   IServiceProvider serviceProvider, 
                                                   ILogger<LogUserAPIExceptionsConsumerService> logger)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password,
                Port = _rabbitMqSetting.Port,
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartConsuming(RabbitMQQueues.UserLogAPI, stoppingToken);
            await Task.CompletedTask;
        }


        private void StartConsuming(string queueName, CancellationToken cancellationToken)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, 
                                  autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                bool processedSuccessfully = false;
                try
                {
                    processedSuccessfully = await ProcessMessageAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while processing message from queue {queueName}: {ex}");
                }

                if (processedSuccessfully)
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }


        private async Task<bool> ProcessMessageAsync(string message)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {

                    var logexceptionService = scope.ServiceProvider.GetRequiredService<ILogExceptionService>();
                    var obj = JsonConvert.DeserializeObject<LogExceptionsEvent>(message);

                    if (obj is not null)
                    {
                        await logexceptionService.AddAsync("userLogAPI", obj);
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                return false;
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

    }
}
