using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using System.Text;
using ToDoList.Domain.Entities;
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
            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await _next(context);

            sw.Stop();

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);

            var log = new RequestLog
            {
                Method = method,
                Path = path,
                Controller = controller,
                Action = action,
                RequestBody = requestBody,
                StatusCode = context.Response.StatusCode,
                ResponseTimeMs = sw.ElapsedMilliseconds,
                CreatedAt = DateTime.UtcNow
            };

            db.RequestLogs.Add(log);
            await db.SaveChangesAsync();

            _logger.LogInformation("Logged to DB: {method} {path} ({status})", method, path, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logging");
            throw;
        }
    }
}

