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

        userGroup.MapPut("/update-user", [Authorize]
        async (UserGetDto dto, [FromServices] IUserService userService) =>
        {
            await userService.UpdateAsync(dto);

            return Results.NoContent();
        })
        .WithName("UpdateUser");


    }
}
