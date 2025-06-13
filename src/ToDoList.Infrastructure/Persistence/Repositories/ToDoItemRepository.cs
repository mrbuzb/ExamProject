using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Repositories;

public class ToDoItemRepository : IToDoItemRepository
{
    public Task DeleteToDoItemByIdAsync(long id, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<long> InsertToDoItemAsync(ToDoItem toDoItem)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ToDoItem>> SearchToDoItemsAsync(string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ToDoItem>> SelectAllToDoItemsByUserIdAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ToDoItem>> SelectByDueDateAsync(DateTime dueDate, long userId)
    {
        throw new NotImplementedException();
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
