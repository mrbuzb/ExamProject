using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;

namespace ToDoList.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        // Sign Up
        group.MapPost("/signUp", async (
            [FromBody] UserCreateDto dto,
            [FromServices] IAuthService authService) =>
        {
            var userId = await authService.SignUpUserAsync(dto);
            return Results.Ok(userId);
        })
        .WithName("SignUp");

        // Login
        group.MapPost("/login", async (
            [FromBody] UserLoginDto dto,
            [FromServices] IAuthService authService) =>
        {
            var result = await authService.LoginUserAsync(dto);
            return Results.Ok(result);
        })
        .WithName("Login");

        // Refresh Token
        group.MapPost("/refreshToken", async (
            [FromBody] RefreshRequestDto dto,
            [FromServices] IAuthService authService) =>
        {
            var result = await authService.RefreshTokenAsync(dto);
            return Results.Ok(result);
        })
        .WithName("RefreshToken");

        // Log Out
        group.MapDelete("/logOut", async (
            [FromQuery] string token,
            [FromServices] IAuthService authService) =>
        {
            await authService.LogOutAsync(token);
            return Results.NoContent();
        })
        .WithName("LogOut");
    }
}
