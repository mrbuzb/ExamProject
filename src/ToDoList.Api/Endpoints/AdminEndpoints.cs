using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/admin")
            .WithTags("Admin Endpoints");

        adminGroup.MapGet("/get-all-users",
            [Authorize(Roles = "Admin,SuperAdmin")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        async ([FromServices] IUserService userService) =>
            {
                var users = await userService.GetAllUsersAsync();
                return Results.Ok(users);
            })
            .WithName("GetAllUsers");
    }
}
