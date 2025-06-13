using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IToDoItemRepository
{
    Task<long> InsertToDoItemAsync(ToDoItem toDoItem);
    Task DeleteToDoItemByIdAsync(long id, long userId);
    Task UpdateToDoItemAsync(ToDoItem toDoItem);
    Task<ICollection<ToDoItem>> SelectAllToDoItemsByUserIdAsync();
    Task<ToDoItem> SelectToDoItemByIdAsync(long id, long userId);
    Task<ICollection<ToDoItem>> SelectByDueDateAsync(DateTime dueDate, long userId);
    Task<ICollection<ToDoItem>> SelectCompletedAsync();
    Task<ICollection<ToDoItem>> SelectIncompletedAsync();
    Task<ICollection<ToDoItem>> SearchToDoItemsAsync(string keyword);
    Task<ICollection<ToDoItem>> SelectOverdueItemsAsync();
    Task<int> SelectTotalCountAsync();
}
