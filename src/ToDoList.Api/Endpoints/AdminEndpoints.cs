using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;

namespace ToDoList.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/admin")
            .WithTags("Admin Endpoints");

        // 👮 Admin rolidagi foydalanuvchilar uchun barcha userlarni olish
        userGroup.MapGet("/get-all-users",
            [Authorize(Roles = "Admin, SuperAdmin")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        async ([FromServices] IUserService _userService) =>
            {
                var users = await _userService.GetAllUsersAsync();
                return Results.Ok(users);
            })
            .WithName("GetAllUsers");

        // 🔍 ToDo itemlarni keyword bo‘yicha qidirish
        app.MapGet("/api/todos/search", async (
    string keyword,
    [FromServices] IToDoItemService service) =>
        {
            var result = await service.SearchToDoItemsAsync(keyword);
            return Results.Ok(result);
        });


        // 📅 ToDo itemlarni sanaga qarab filterlash
        app.MapGet("/todo/filter-by-date", async (
    DateTime dueDate,
    [FromServices] IToDoItemRepository repository,
    HttpContext httpContext) =>
        {
            // ...
            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var items = await repository.SelectByDueDateAsync(dueDate, userId);
            return Results.Ok(items);
        });


        
    }
}
