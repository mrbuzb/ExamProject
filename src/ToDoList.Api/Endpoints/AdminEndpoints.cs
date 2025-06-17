using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Serveces;
using ToDoList.Application.Services;

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

        app.MapGet("/api/todos/search", async (
            [FromQuery] string keyword,
            IToDoItemService service) =>
        {
            var result = await service.SearchToDoItemsAsync(keyword);
            return Results.Ok(result);
        });

        app.MapGet("/todo/filter-by-date", async (
         [FromQuery] DateTime dueDate,
         [FromServices] IToDoItemRepository repository,
         HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var items = await repository.SelectByDueDateAsync(dueDate, userId);
            return Results.Ok(items);
        })
        .WithName("FilterToDoByDueDate")
        .WithTags("ToDoItems");

        app.MapGet("/todo/completed", async (
         [FromServices] IToDoItemRepository repository,
          HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var items = await repository.SelectCompletedAsync();
            return Results.Ok(items);
        })
        .WithName("GetCompletedToDos")
        .WithTags("ToDoItems");

        app.MapGet("/todo/incompleted", async (
         [FromServices] IToDoItemRepository repository,
         HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var items = await repository.SelectIncompletedAsync();
            return Results.Ok(items);
        })
        .WithName("GetIncompletedToDos")
        .WithTags("ToDoItems");

    }
}
