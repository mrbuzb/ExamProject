namespace ToDoList.Api.Middlewares;

public class SuccessRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SuccessRequestLoggingMiddleware> _logger;

    public SuccessRequestLoggingMiddleware(RequestDelegate next, ILogger<SuccessRequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        // Faqat muvaffaqiyatli statuslarni log qilamiz
        if (context.Response.StatusCode == 200 || context.Response.StatusCode == 201)
        {
            var userId = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonymous";

            _logger.LogInformation("✅ SUCCESS | User: {User} | {Method} {Path} | Status: {StatusCode} | Time: {Time}",
                userId,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                DateTime.UtcNow);
        }

        await responseBody.CopyToAsync(originalBodyStream);
    }
}

