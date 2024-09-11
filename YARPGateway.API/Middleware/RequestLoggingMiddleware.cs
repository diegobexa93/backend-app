using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString(); // Generate a unique request ID
        var requestTimestamp = DateTime.UtcNow;

        // Log request details
        await LogRequest(context, requestId, requestTimestamp);

        // Capture and log response details
        await LogResponse(context, requestId);
    }

    private async Task LogRequest(HttpContext context, string requestId, DateTime requestTimestamp)
    {
        // Enable buffering to allow multiple reads of the request body
        context.Request.EnableBuffering();

        string requestBody = await ReadRequestBody(context);

        Console.WriteLine($"Request ID: {requestId}");
        Console.WriteLine($"Request URL: {context.Request.GetDisplayUrl()}");
        Console.WriteLine($"Request Timestamp: {requestTimestamp}");

        // Log request headers
        Console.WriteLine("Request Headers:");
        foreach (var header in context.Request.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        Console.WriteLine($"Request Body: {requestBody}");

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

    private async Task LogResponse(HttpContext context, string requestId)
    {
        var originalResponseBody = context.Response.Body;

        // Capture the response body in a memory stream
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            // Call the next middleware in the pipeline
            await _next(context);

            // Log the response after it has been processed
            await LogResponseDetails(context, requestId);

            // Copy the response back to the original stream
            await responseBody.CopyToAsync(originalResponseBody);
        }
    }

    private async Task LogResponseDetails(HttpContext context, string requestId)
    {
        context.Response.Body.Position = 0;
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Position = 0; // Reset again before sending it to the client

        Console.WriteLine($"Response ID: {requestId}");
        Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");

        // Log response headers
        Console.WriteLine("Response Headers:");
        foreach (var header in context.Response.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        Console.WriteLine($"Response Body: {responseBody}");
    }
}
