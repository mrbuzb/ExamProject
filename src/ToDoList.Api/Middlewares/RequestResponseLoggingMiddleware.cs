using ToDoList.Infrastructure.Persistence;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        // Swagger va statik fayllarni loglamaslik
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/favicon.ico"))
        {
            await _next(context);
            return;
        }

        try
        {
            context.Request.EnableBuffering();
            var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
            context.Request.Body.Position = 0;

            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} Body: {requestBodyText}");

            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Response: {context.Response.StatusCode} Body: {responseBodyText}");

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
            responseBody.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logging");
            throw;
        }
    }

}
