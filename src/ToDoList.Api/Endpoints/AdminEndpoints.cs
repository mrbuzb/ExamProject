using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Serveces;

namespace ToDoList.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/admin")
           .WithTags("Admin Endpoints");

        userGroup.MapGet("/get-all-users", [Authorize(Roles = "Admin, SuperAdmin")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        async (IUserServices _userService) =>
        {
            var users = await _userService.GetAllUsersAsync();
            return Results.Ok(users);
        })
            .WithName("GetAllUsers");

    }
}
