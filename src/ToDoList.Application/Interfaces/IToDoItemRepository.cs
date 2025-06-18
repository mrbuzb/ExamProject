using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.DTOs;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IToDoItemRepository
{
    Task<long> InsertToDoItemAsync(ToDoItem toDoItem);
    Task DeleteToDoItemByIdAsync(long id, long userId);
    Task<int> DeleteCompletedAsync(long userId);

    Task UpdateToDoItemAsync(ToDoItem toDoItem);
    Task<ICollection<ToDoItem>> SelectAllToDoItemsByUserIdAsync(long userId);
    Task<ToDoItem> SelectToDoItemByIdAsync(long id, long userId);
    Task<ICollection<ToDoItem>> SelectByDueDateAsync(DateTime dueDate, long userId);
    Task<ICollection<ToDoItem>> SelectCompletedAsync();
    Task<ICollection<ToDoItem>> SelectIncompletedAsync();
    Task<ICollection<ToDoItem>> SearchToDoItemsAsync(string keyword);
    Task<ICollection<ToDoItem>> SelectOverdueItemsAsync(long userId);
    Task<int> SelectTotalCountAsync();
    Task MarkAsCompletedAsync(long id, long userId);
    Task SetDueDateAsync(long id, long userId, DateTime dueDate);
    Task<ToDoSummaryDto> GetSummaryAsync(long userId);

}
