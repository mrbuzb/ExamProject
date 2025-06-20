using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using ToDoList.Api.Middlewares;
using ToDoList.Core.Errors;

namespace ToDoList.Tests
{
    public class UnitTestForExceptionHandlingMiddleware
    {
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
        private readonly DefaultHttpContext _httpContext;

        public UnitTestForExceptionHandlingMiddleware()
        {
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        [Fact]
        public async Task InvokeAsync_CallsNextDelegate_WhenNoException()
        {
            // Arrange
            var wasCalled = false;
            RequestDelegate next = ctx =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.True(wasCalled);
            Assert.Equal(200, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_HandlesEntityNotFoundException_Returns404()
        {
            // Arrange
            var exception = new EntityNotFoundException("Entity not found");
            RequestDelegate next = ctx => throw exception;
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(404, _httpContext.Response.StatusCode);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await JsonSerializer.DeserializeAsync<JsonElement>(_httpContext.Response.Body);
            Assert.Equal(404, response.GetProperty("StatusCode").GetInt32());
            Assert.Equal("Entity not found", response.GetProperty("Detail").GetString());
        }

        [Theory]
        [InlineData(typeof(AuthException))]
        [InlineData(typeof(UnauthorizedException))]
        public async Task InvokeAsync_HandlesAuthExceptions_Returns401(Type exceptionType)
        {
            // Arrange
            var exception = (Exception)Activator.CreateInstance(exceptionType, "Auth error");
            RequestDelegate next = ctx => throw exception;
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(401, _httpContext.Response.StatusCode);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await JsonSerializer.DeserializeAsync<JsonElement>(_httpContext.Response.Body);
            Assert.Equal(401, response.GetProperty("StatusCode").GetInt32());
            Assert.Equal("Auth error", response.GetProperty("Detail").GetString());
        }

        [Theory]
        [InlineData(typeof(ForbiddenException))]
        [InlineData(typeof(NotAllowedException))]
        public async Task InvokeAsync_HandlesForbiddenExceptions_Returns403(Type exceptionType)
        {
            // Arrange
            var exception = (Exception)Activator.CreateInstance(exceptionType, "Forbidden error");
            RequestDelegate next = ctx => throw exception;
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(403, _httpContext.Response.StatusCode);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await JsonSerializer.DeserializeAsync<JsonElement>(_httpContext.Response.Body);
            Assert.Equal(403, response.GetProperty("StatusCode").GetInt32());
            Assert.Equal("Forbidden error", response.GetProperty("Detail").GetString());
        }

        [Fact]
        public async Task InvokeAsync_HandlesUnknownException_Returns500()
        {
            // Arrange
            var exception = new Exception("Unknown error");
            RequestDelegate next = ctx => throw exception;
            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(500, _httpContext.Response.StatusCode);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await JsonSerializer.DeserializeAsync<JsonElement>(_httpContext.Response.Body);
            Assert.Equal(500, response.GetProperty("StatusCode").GetInt32());
            Assert.Equal("Unknown error", response.GetProperty("Detail").GetString());
        }
    }
}