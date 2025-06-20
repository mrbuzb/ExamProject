using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using ToDoList.Api;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;

namespace ToDoList.Tests;

public class AdminEndpointsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AdminEndpointsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove real services and add mocks
                var roleServiceMock = new Mock<IRoleService>();
                var userServiceMock = new Mock<IUserService>();

                // Setup mock behaviors
                roleServiceMock.Setup(x => x.GetAllUsersByRoleAsync(It.IsAny<string>()))
                    .ReturnsAsync(new List<UserGetDto> { new UserGetDto { UserId = 1, UserName = "testuser", Role = "Admin" } });

                userServiceMock.Setup(x => x.DeleteUserByIdAsync(It.IsAny<long>(), It.IsAny<string>()))
                    .Returns(Task.CompletedTask);

                userServiceMock.Setup(x => x.UpdateUserRoleAsync(It.IsAny<long>(), It.IsAny<string>()))
                    .Returns(Task.CompletedTask);

                services.AddSingleton(roleServiceMock.Object);
                services.AddSingleton(userServiceMock.Object);

                // Add fake authentication
                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            });
        });
    }

    [Fact]
    public async Task GetAllUsersByRole_ReturnsOk_ForAdmin()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "TestAdmin");

        var response = await client.GetAsync("/api/admin/get-all-users-by-role?role=Admin");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var users = await response.Content.ReadFromJsonAsync<List<UserGetDto>>();
        Assert.NotNull(users);
        Assert.Single(users);
        Assert.Equal("Admin", users[0].Role);
    }

    [Fact]
    public async Task DeleteUserById_ReturnsOk_ForAdmin()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "TestAdmin");

        var response = await client.DeleteAsync("/api/admin/delete-user-by-id?userId=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUserRole_ReturnsOk_ForSuperAdmin()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "TestSuperAdmin");

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("userId", "1"),
            new KeyValuePair<string, string>("userRole", "Admin")
        });

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), "/api/admin/update-user-role?userId=1&userRole=Admin")
        {
            Content = content
        };

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

// Fake authentication handler for testing
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var role = Context.Request.Headers["Authorization"].ToString() == "TestSuperAdmin" ? "SuperAdmin" : "Admin";
        var claims = new[] { new Claim(ClaimTypes.Name, "testuser"), new Claim(ClaimTypes.Role, role) };
        var identity = new ClaimsIdentity(claims, "TestScheme");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}