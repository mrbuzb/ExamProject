using ToDoList.Application.Dtos;
using ToDoList.Application.DTOs;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public interface IToDoItemService
{
    Task<ICollection<ToDoItemGetDto>> SearchToDoItemsAsync(string keyword, long userId);
    Task<ICollection<ToDoItemGetDto>> GetAllToDoItemsByUserIdAsync(long userId);
    Task<ICollection<ToDoItemGetDto>> GetByDueDateAsync(DateTime dueDate, long userId);
    Task<ICollection<ToDoItemGetDto>> GetOverdueItemsAsync(long userId);
    Task<long> InsertToDoItemAsync(ToDoItemCreateDto toDoItem, long userId);
    Task DeleteToDoItemByIdAsync(long id, long userId);
    Task<int> DeleteCompletedAsync(long userId);
    Task UpdateToDoItemAsync(ToDoItemUpdateDto toDoItem, long userId);
    Task<ICollection<ToDoItemGetDto>> SelectAllToDoItemsByUserIdAsync(long userId);
    Task<ToDoItemGetDto> SelectToDoItemByIdAsync(long id, long userId);
    Task<ICollection<ToDoItemGetDto>> SelectByDueDateAsync(DateTime dueDate, long userId);
    Task<ICollection<ToDoItemGetDto>> SelectCompletedAsync(long userId);
    Task<ICollection<ToDoItemGetDto>> SelectIncompletedAsync(long userId);
    Task<ICollection<ToDoItemGetDto>> SelectOverdueItemsAsync(long userId);
    int SelectTotalCountAsync(long userId);
    Task MarkAsCompletedAsync(long id, long userId);
    Task SetDueDateAsync(long id, long userId, DateTime dueDate);
    Task<ToDoSummaryDto> GetSummaryAsync(long userId);
}
