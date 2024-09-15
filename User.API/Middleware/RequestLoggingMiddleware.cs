using BaseShare.Common.Interface.Communication;
using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public RequestLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task Invoke(HttpContext context)
    {
        var requestId = Guid.NewGuid(); // Generate a unique request ID
        var requestTimestamp = DateTime.UtcNow;

        var traceRequestEvent = new TraceRequestEvent(requestId, requestTimestamp);

        // Log request details
        await LogRequest(context, traceRequestEvent);

        // Capture the original response body
        var originalResponseBody = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody; // Use a memory stream to capture the response

            // Call the next middleware in the pipeline
            await _next(context);

            // Log the response details after the response has been generated
            await LogResponseDetails(context, traceRequestEvent);

            // Copy the response body back to the original stream
            responseBody.Seek(0, SeekOrigin.Begin); // Reset the position to read the response
            await responseBody.CopyToAsync(originalResponseBody); // Copy the captured response to the original response
        }

        try
        {
            // Resolve the IMessageBrokerLog service and send the log to another API
            var messageBrokerLog = _serviceProvider.GetRequiredService<IMessageBrokerLog>();
            _ = messageBrokerLog.CreateTraceUserAPI(traceRequestEvent);
        }
        catch (Exception logEx)
        {
            Console.WriteLine($"Failed to log exception: {logEx.Message}");
        }
    }

    private async Task LogRequest(HttpContext context, TraceRequestEvent traceRequestEvent)
    {
        // Enable buffering to allow multiple reads of the request body
        context.Request.EnableBuffering();

        // Capture request URL
        traceRequestEvent.RequestURL = context.Request.GetDisplayUrl();

        // Capture request headers
        foreach (var header in context.Request.Headers)
        {
            traceRequestEvent.RequestHeaders.Add(header.Key, header.Value.ToString());
        }

        // Capture request body
        string requestBody = await ReadRequestBody(context);
        traceRequestEvent.RequestBody = requestBody;

        // Reset the position of the request body for further processing by other middleware
        context.Request.Body.Position = 0;
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            return await reader.ReadToEndAsync();
        }
    }

    private async Task LogResponseDetails(HttpContext context, TraceRequestEvent traceRequestEvent)
    {
        // Capture response status code
        traceRequestEvent.TraceResponse.ResponseStatusCode = context.Response.StatusCode;

        // Capture response headers
        foreach (var header in context.Response.Headers)
        {
            traceRequestEvent.TraceResponse.ResponseHeaders.Add(header.Key, header.Value.ToString());
        }

        // Capture response body
        context.Response.Body.Seek(0, SeekOrigin.Begin); // Reset to the beginning of the stream
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        traceRequestEvent.TraceResponse.ResponseBody = responseBody;

        // Reset the response body position for the next middleware or final response
        context.Response.Body.Seek(0, SeekOrigin.Begin);
    }
}
