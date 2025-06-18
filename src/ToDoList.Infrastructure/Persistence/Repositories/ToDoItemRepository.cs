using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (item is not null)
        {
            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }


    public Task<long> InsertToDoItemAsync(ToDoItem toDoItem)
    {
        throw new NotImplementedException();
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


    public Task<ICollection<ToDoItem>> SelectCompletedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ToDoItem>> SelectIncompletedAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<ToDoItem>> SelectOverdueItemsAsync(long userId)
    {
        var now = DateTime.UtcNow;
        return await _context.ToDoItems
            .Where(x => x.DueDate < now && !x.IsCompleted && x.UserId == userId)
            .ToListAsync();
    }


    public Task<ToDoItem> SelectToDoItemByIdAsync(long id, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<int> SelectTotalCountAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateToDoItemAsync(ToDoItem toDoItem)
    {
        throw new NotImplementedException();
    }

    public async Task MarkAsCompletedAsync(long id, long userId)
    {
        var item = await _context.ToDoItems
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (item is not null)
        {
            item.IsCompleted = true;
            await _context.SaveChangesAsync();
        }
    }

}
