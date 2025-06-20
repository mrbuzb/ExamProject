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
    Task UpdateToDoItemAsync(ToDoItem toDoItem, long userId);
    Task<ICollection<ToDoItem>> SelectAllToDoItemsByUserIdAsync(long userId);
    Task<ToDoItem> SelectToDoItemByIdAsync(long id, long userId);
    Task<ICollection<ToDoItem>> SelectByDueDateAsync(DateTime dueDate, long userId);
    Task<ICollection<ToDoItem>> SelectCompletedAsync(long userId);
    Task<ICollection<ToDoItem>> SelectIncompletedAsync(long userId);
    Task<ICollection<ToDoItem>> SearchToDoItemsAsync(string keyword, long userId);
    Task<ICollection<ToDoItem>> SelectOverdueItemsAsync(long userId);
    int SelectTotalCountAsync(long userId);
    Task MarkAsCompletedAsync(long id, long userId);
    Task SetDueDateAsync(long id, long userId, DateTime dueDate);
    Task<ToDoSummaryDto> GetSummaryAsync(long userId);
}
