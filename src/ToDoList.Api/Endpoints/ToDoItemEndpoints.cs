using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Api.Endpoints;

public static class ToDoItemEndpoints
{
    public static void MapToDoItemEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/to-do-item")
            .WithTags("ToDoItem Endpoints");


        app.MapGet("/api/todos/search", async (
            [FromQuery] string keyword,
            [FromServices] IToDoItemService service) =>
        {
            var result = await service.SearchToDoItemsAsync(keyword);
            return Results.Ok(result);
        });

        app.MapGet("/todo/filter-by-date", async (
            [FromQuery] DateTime dueDate,
            [FromServices] IToDoItemService service,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var items = await service.GetByDueDateAsync(dueDate, userId);
            return Results.Ok(items);
        })
        .WithName("FilterToDoByDueDate")
        .WithTags("ToDoItems");


        app.MapGet("/todo/overdue", async (
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var items = await repository.SelectOverdueItemsAsync(userId);
            return Results.Ok(items);
        })
        .WithName("GetOverdueToDos")
        .WithTags("ToDoItems");




        app.MapGet("/api/todos/user/{userId:long}", async (
            long userId,
            [FromServices] IToDoItemService service) =>
        {
            var todos = await service.GetAllToDoItemsByUserIdAsync(userId);
            return Results.Ok(todos);
        })
        .WithName("GetToDosByUserId")
        .WithTags("ToDos");





        app.MapDelete("/todo/{id:long}", async (
            long id,
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            await repository.DeleteToDoItemByIdAsync(id, userId);
            return Results.NoContent();
        })
        .WithName("DeleteToDoItem")
        .WithTags("ToDoItems");





        app.MapPost("/todo/complete/{id:long}", async (
           long id,
           [FromServices] IToDoItemRepository repository,
           HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            await repository.MarkAsCompletedAsync(id, userId);
            return Results.Ok("ToDo marked as completed.");
        })
       .WithName("MarkToDoCompleted")
       .WithTags("ToDoItems");





        app.MapPut("/todo/update", async (
            [FromBody] ToDoItem updatedItem,
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            updatedItem.UserId = userId;

            try
            {
                await repository.UpdateToDoItemAsync(updatedItem);
                return Results.Ok("ToDo item updated successfully");
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound("ToDo item not found");
            }
        })
        .WithName("UpdateToDoItem")
        .WithTags("ToDoItems");




        app.MapPut("/todo/{id}/set-due-date", async (
            long id,
            [FromBody] DateTime dueDate,
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            try
            {
                await repository.SetDueDateAsync(id, userId, dueDate);
                return Results.Ok($"Due date set to {dueDate}");
            }
            catch (Exception ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("SetToDoDueDate")
        .WithTags("ToDoItems");




        app.MapDelete("/todo/delete-completed", async (
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var deletedCount = await repository.DeleteCompletedAsync(userId);

            return deletedCount == 0
                ? Results.NotFound("No completed ToDos found to delete.")
                : Results.Ok($"{deletedCount} completed ToDos deleted.");
        })
        .WithName("DeleteCompletedToDos")
        .WithTags("ToDoItems");




        app.MapGet("/todo/summary", async (
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            var summary = await repository.GetSummaryAsync(userId);
            return Results.Ok(summary);
        })
        .WithName("GetToDoSummary")
        .WithTags("ToDoItems");









        app.MapPost("/todo", async (
            [FromBody] ToDoItem toDoItem,
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            toDoItem.UserId = userId;
            var id = await repository.InsertToDoItemAsync(toDoItem);
            return Results.Created($"/todo/{id}", new { Id = id });
        })
        .WithName("CreateToDoItem")
        .WithTags("ToDoItems");



        app.MapGet("/todo/{id:long}", async (
            long id,
            [FromServices] IToDoItemRepository repository,
            HttpContext httpContext) =>
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !long.TryParse(userIdClaim.Value, out var userId))
                return Results.BadRequest("Invalid user ID");

            try
            {
                var item = await repository.SelectToDoItemByIdAsync(id, userId);
                return Results.Ok(item);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("GetToDoItemById")
        .WithTags("ToDoItems");





        app.MapGet("/todo/count", async (
            [FromServices] IToDoItemRepository repository) =>
        {
            var count = await repository.SelectTotalCountAsync();
            return Results.Ok(count);
        })
        .WithName("GetTotalToDoCount")
        .WithTags("ToDoItems");

    }
}
