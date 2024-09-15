using BaseShare.Common.Domain;
using BaseShare.Common.Repositories.Mongo;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Interface;
using EventBusRabbitMQ.Services;
using LogMessageBroker.API.Interface;
using LogMessageBroker.API.Services;
using LogMessageBroker.API.Worker.UserAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped(typeof(IRabbitMQPublisher<>), typeof(RabbitMQPublisher<>));
builder.Services.AddHostedService<UserLogExceptionsConsumerService>();
builder.Services.AddHostedService<UserTraceConsumerService>();


builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Register MongoContext as a scoped service for DI
builder.Services.AddScoped(typeof(MongoContext<>));
builder.Services.AddScoped<ILogExceptionService, LogExceptionService>();
builder.Services.AddScoped<ITraceService, TraceService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
