using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        var request = context.Request;
        var method = request.Method;
        var path = request.Path + request.QueryString;
        var controller = context.GetEndpoint()?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ControllerName ?? "Unknown";
        var action = context.GetEndpoint()?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ActionName ?? "Unknown";

        string requestBody = "";
        if (request.ContentLength > 0 && request.Body.CanRead)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        try
        {
            // Response'ni tutish uchun original streamni o‘zgartiramiz
            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await _next(context);

            sw.Stop();

            newBody.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(newBody).ReadToEnd();
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);

            var log = $"""
            🔄 HTTP Request
            📅 Time       : {DateTime.Now:yyyy-MM-dd HH:mm:ss}
            🔗 Path       : {method} {path}
            🎯 Controller : {controller}
            📍 Action     : {action}
            📥 Body       : {(!string.IsNullOrWhiteSpace(requestBody) ? requestBody : "None")}
            ✅ Status     : {context.Response.StatusCode}
            ⏱️ Duration   : {sw.ElapsedMilliseconds} ms
            """;

            _logger.LogInformation(log);
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, $"""
            💥 EXCEPTION in {method} {path}
            ❌ Controller: {controller}, Action: {action}
            ⏱️ Duration  : {sw.ElapsedMilliseconds} ms
            🧾 Request Body: {requestBody}
            """);

            throw;
        }
    }
}
