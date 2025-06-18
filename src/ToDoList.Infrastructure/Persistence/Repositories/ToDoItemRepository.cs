using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Repositories;

public class ToDoItemRepository : IToDoItemRepository
{
    private readonly AppDbContext _context;
    public ToDoItemRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteToDoItemByIdAsync(long id, long userId)
    {
        var item = await _context.ToDoItems
            .FirstOrDefaultAsync(x => x.ToDoItemId == id && x.UserId == userId);

        if (item is not null)
        {
            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<long> InsertToDoItemAsync(ToDoItem toDoItem)
    {
        await _context.ToDoItems.AddAsync(toDoItem);
        await _context.SaveChangesAsync();
        return toDoItem.ToDoItemId;
    }


    public async Task<ICollection<ToDoItem>> SearchToDoItemsAsync(string keyword)
    {
        return await _context.ToDoItems
            .Where(x => x.Title.Contains(keyword) || x.Description.Contains(keyword))
            .ToListAsync();
    }


    public async Task<ICollection<ToDoItem>> SelectAllToDoItemsByUserIdAsync(long userId)
    {
        return await _context.ToDoItems
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }


    public async Task<ICollection<ToDoItem>> SelectByDueDateAsync(DateTime dueDate, long userId)
    {
        var items = await _context.ToDoItems
            .Where(x => x.DueDate.Date == dueDate.Date && x.UserId == userId)
            .ToListAsync();

        return items;
    }


    public async Task<ICollection<ToDoItem>> SelectCompletedAsync()
    {
        return await _context.ToDoItems
            .Where(x => x.IsCompleted)
            .ToListAsync();
    }


    public async Task<ICollection<ToDoItem>> SelectIncompletedAsync()
    {
        return await _context.ToDoItems
            .Where(x => !x.IsCompleted)
            .ToListAsync();
    }

    public async Task<ICollection<ToDoItem>> SelectOverdueItemsAsync(long userId)
    {
        var now = DateTime.UtcNow;
        return await _context.ToDoItems
            .Where(x => x.DueDate < now && !x.IsCompleted && x.UserId == userId)
            .ToListAsync();
    }


    public async Task<ToDoItem> SelectToDoItemByIdAsync(long id, long userId)
    {
        var item = await _context.ToDoItems
            .FirstOrDefaultAsync(x => x.ToDoItemId == id && x.UserId == userId);

        if (item is null)
            throw new KeyNotFoundException("ToDo item not found");

        return item;
    }


    public async Task<int> SelectTotalCountAsync()
    {
        return await _context.ToDoItems.CountAsync();
    }


    public async Task UpdateToDoItemAsync(ToDoItem toDoItem)
    {
        var existingItem = await _context.ToDoItems
            .FirstOrDefaultAsync(x => x.ToDoItemId == toDoItem.ToDoItemId && x.UserId == toDoItem.UserId);

        if (existingItem is null)
            throw new KeyNotFoundException("ToDo item not found");

        existingItem.Title = toDoItem.Title;
        existingItem.Description = toDoItem.Description;
        existingItem.IsCompleted = toDoItem.IsCompleted;
        existingItem.DueDate = toDoItem.DueDate;

        await _context.SaveChangesAsync();
    }


    public async Task MarkAsCompletedAsync(long id, long userId)
    {
        var item = await _context.ToDoItems
            .FirstOrDefaultAsync(x => x.ToDoItemId == id && x.UserId == userId);

        if (item is not null)
        {
            item.IsCompleted = true;
            await _context.SaveChangesAsync();
        }
    }


    public async Task SetDueDateAsync(long id, long userId, DateTime dueDate)
    {
        var item = await _context.ToDoItems.FirstOrDefaultAsync(x => x.ToDoItemId == id && x.UserId == userId);
        if (item is null)
            throw new Exception("ToDo item not found");

        item.DueDate = dueDate;
        await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteCompletedAsync(long userId)
    {
        var completedItems = await _context.ToDoItems
            .Where(x => x.UserId == userId && x.IsCompleted)
            .ToListAsync();

        if (completedItems.Count == 0)
            return 0;

        _context.ToDoItems.RemoveRange(completedItems);
        return await _context.SaveChangesAsync();
    }

    public async Task<ToDoSummaryDto> GetSummaryAsync(long userId)
    {
        var now = DateTime.UtcNow;

        var total = await _context.ToDoItems.CountAsync(x => x.UserId == userId);
        var completed = await _context.ToDoItems.CountAsync(x => x.UserId == userId && x.IsCompleted);
        var incompleted = await _context.ToDoItems.CountAsync(x => x.UserId == userId && !x.IsCompleted);
        var overdue = await _context.ToDoItems.CountAsync(x => x.UserId == userId && !x.IsCompleted && x.DueDate < now);

        return new ToDoSummaryDto
        {
            Total = total,
            Completed = completed,
            Incompleted = incompleted,
            Overdue = overdue
        };
    }

}
