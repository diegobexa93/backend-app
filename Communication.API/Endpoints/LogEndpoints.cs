using Communication.API.Interface;
using EventBusRabbitMQ.Events;

namespace Communication.API.Endpoints
{
    public static class LogEndpoints
    {
        public static void MapUserLogEndpoints(this WebApplication app)
        {
            app.MapPost("/api/users", async (IUserLogAPI userLogApi, LogExceptionsEvent logExceptionEvent) =>
            {
                if (logExceptionEvent == null)
                {
                    return Results.BadRequest("User data is required.");
                }

                var log = await userLogApi.CreateLogExceptionsEventsAsync(logExceptionEvent);
                return Results.Created($"/api/LogExceptions/CreateLogUserAPI/{log.Id}", log);
            });
        }
    }
}
