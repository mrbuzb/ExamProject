using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;

namespace ToDoList.Api.Endpoints;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/user")
            .WithTags("User Endpoints");

        // 👤 Foydalanuvchi yaratish
        userGroup.MapPost("/create-user", [Authorize]
        async (UserCreateDto dto, [FromServices] IUserService userService) =>
        {
            var userId = await userService.CreateUserAsync(dto);
            return Results.Created($"/api/user/{userId.UserId}", userId);
        })
.WithName("CreateUser");

        userGroup.MapPut("/update-user", [Authorize]
        async (UserGetDto dto, [FromServices] IUserService userService) =>
        {
            await userService.UpdateAsync(dto);

            return Results.NoContent();
        })
        .WithName("UpdateUser");

    }
}
