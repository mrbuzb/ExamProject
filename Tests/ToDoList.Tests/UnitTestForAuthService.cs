using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using ToDoList.Api;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;

namespace ToDoList.Tests;

public class UnitTestForAuthService : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IAuthService> _authServiceMock;

    public UnitTestForAuthService(WebApplicationFactory<Program> factory)
    {
        _authServiceMock = new Mock<IAuthService>();
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_authServiceMock.Object);
            });
        });
    }

    [Fact]
    public async Task SignUp_ReturnsOk()
    {
        // Arrange
        var user = new UserCreateDto
        {
            FirstName = "John",
            LastName = "Doe",
            UserName = "johndoe",
            Password = "password",
            PhoneNumber = "1234567890",
            Email = "john@example.com"
        };
        _authServiceMock.Setup(s => s.SignUpUserAsync(It.IsAny<UserCreateDto>()))
            .ReturnsAsync(1);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/sign-up", user);

        // Assert
        response.EnsureSuccessStatusCode();
        _authServiceMock.Verify(s => s.SignUpUserAsync(It.Is<UserCreateDto>(u => u.UserName == "johndoe")), Times.Once);
    }

    [Fact]
    public async Task Login_ReturnsOk()
    {
        // Arrange
        var loginDto = new UserLoginDto
        {
            UserName = "johndoe",
            Password = "password"
        };
        var loginResponse = new LoginResponseDto
        {
            AccessToken = "access",
            RefreshToken = "refresh",
            TokenType = "Bearer",
            Expires = 3600
        };
        _authServiceMock.Setup(s => s.LoginUserAsync(It.IsAny<UserLoginDto>()))
            .ReturnsAsync(loginResponse);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.EnsureSuccessStatusCode();
        _authServiceMock.Verify(s => s.LoginUserAsync(It.Is<UserLoginDto>(u => u.UserName == "johndoe")), Times.Once);
    }

    [Fact]
    public async Task RefreshToken_ReturnsOk()
    {
        // Arrange
        var refreshDto = new RefreshRequestDto
        {
            AccessToken = "access",
            RefreshToken = "refresh"
        };
        var loginResponse = new LoginResponseDto
        {
            AccessToken = "new_access",
            RefreshToken = "new_refresh",
            TokenType = "Bearer",
            Expires = 3600
        };
        _authServiceMock.Setup(s => s.RefreshTokenAsync(It.IsAny<RefreshRequestDto>()))
            .ReturnsAsync(loginResponse);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync("/api/auth/refresh-token", refreshDto);

        // Assert
        response.EnsureSuccessStatusCode();
        _authServiceMock.Verify(s => s.RefreshTokenAsync(It.Is<RefreshRequestDto>(r => r.RefreshToken == "refresh")), Times.Once);
    }

    [Fact]
    public async Task LogOut_ReturnsOk()
    {
        // Arrange
        var refreshToken = "refresh";
        _authServiceMock.Setup(s => s.LogOutAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"/api/auth/log-out?refreshToken={refreshToken}");

        // Assert
        response.EnsureSuccessStatusCode();
        _authServiceMock.Verify(s => s.LogOutAsync(refreshToken), Times.Once);
    }
}