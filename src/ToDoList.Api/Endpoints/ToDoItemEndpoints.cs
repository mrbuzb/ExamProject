using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Dtos;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Core.Errors;
using ToDoList.Domain.Entities;

namespace ToDoList.Api.Endpoints;

public static class ToDoItemEndpoints
{
    public static void MapToDoItemEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/to-do-item")
            .WithTags("ToDoItem Endpoints");

        
        app.MapGet("/search", async (string keyword, IToDoItemService service, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();
            var result = await service.SearchToDoItemsAsync(keyword, long.Parse(userId));
            return Results.Ok(result);
        });

        app.MapGet("/filter-by-date", async (DateTime dueDate, IToDoItemService service, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();
            var items = await service.GetByDueDateAsync(dueDate, long.Parse(userId));
            return Results.Ok(items);
        })
        .WithName("FilterToDoByDueDate");


        app.MapGet("/overdue", async (IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();

            var items = await repository.SelectOverdueItemsAsync(long.Parse(userId));
            return Results.Ok(items);
        })
        .WithName("GetOverdueToDos");




        app.MapGet("/get-all", async (IToDoItemService service, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();
            var todos = await service.GetAllToDoItemsByUserIdAsync(long.Parse(userId));
            return Results.Ok(todos);
        })
        .WithName("GetToDosByUserId");





        app.MapDelete("/delete", async (long id, IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();

            await repository.DeleteToDoItemByIdAsync(id, long.Parse(userId));
            return Results.NoContent();
        })
        .WithName("DeleteToDoItem");





        app.MapPatch("/mark-as-compleated", async (long id, IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();

            await repository.MarkAsCompletedAsync(id, long.Parse(userId));
            return Results.Ok("ToDo marked as completed.");
        })
       .WithName("MarkToDoCompleted");





        app.MapPut("/update", async (ToDoItemUpdateDto updatedItem, IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();

            try
            {
                await repository.UpdateToDoItemAsync(updatedItem, long.Parse(userId));
                return Results.Ok("ToDo item updated successfully");
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound("ToDo item not found");
            }
        })
        .WithName("UpdateToDoItem");




        app.MapPut("/set-due-date", async (long id, DateTime dueDate, IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();

            try
            {
                await repository.SetDueDateAsync(id, long.Parse(userId), dueDate);
                return Results.Ok($"Due date set to {dueDate}");
            }
            catch (Exception ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("SetToDoDueDate");




        app.MapDelete("/delete-all-completed", async (IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();

            var deletedCount = await repository.DeleteCompletedAsync(long.Parse(userId));

            return deletedCount == 0
                ? Results.NotFound("No completed ToDos found to delete.")
                : Results.Ok($"{deletedCount} completed ToDos deleted.");
        })
        .WithName("DeleteCompletedToDos");




        app.MapGet("/get-summary", async (IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();
            var summary = await repository.GetSummaryAsync(long.Parse(userId));
            return Results.Ok(summary);
        })
        .WithName("GetToDoSummary");


        app.MapPost("/create", async (ToDoItemCreateDto toDoItem, IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();
            var id = await repository.InsertToDoItemAsync(toDoItem, long.Parse(userId));
            return Results.Created($"/todo/{id}", new { Id = id });
        })
        .WithName("CreateToDoItem");



        app.MapGet("select-by-id", async (long id, IToDoItemService repository, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();
            try
            {
                var item = await repository.SelectToDoItemByIdAsync(id, long.Parse(userId));
                return Results.Ok(item);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("GetToDoItemById");





        app.MapGet("/count", async (IToDoItemService service, HttpContext context) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            if (userId is null) throw new AuthException();
            var count = service.SelectTotalCountAsync(long.Parse(userId));
            return Results.Ok(count);
        })
        .WithName("GetTotalToDoCount");

    }
}
