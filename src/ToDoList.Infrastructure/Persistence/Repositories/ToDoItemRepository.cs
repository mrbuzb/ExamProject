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
    public Task DeleteToDoItemByIdAsync(long id, long userId)
    {
        throw new NotImplementedException();
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


    public Task<ICollection<ToDoItem>> SelectAllToDoItemsByUserIdAsync()
    {
        throw new NotImplementedException();
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

    public Task<ICollection<ToDoItem>> SelectOverdueItemsAsync()
    {
        throw new NotImplementedException();
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
}
