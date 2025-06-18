using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Dtos;
using ToDoList.Application.Interfaces;

namespace ToDoList.Application.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _toDoItemRepository;

        public ToDoItemService(IToDoItemRepository toDoItemRepository)
        {
            _toDoItemRepository = toDoItemRepository;
        }
        public async Task<ICollection<ToDoItemGetDto>> SearchToDoItemsAsync(string keyword)
        {
            var items = await _toDoItemRepository.SearchToDoItemsAsync(keyword);

            return items.Select(x => new ToDoItemGetDto
            {
                ToDoItemId = x.ToDoItemId,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate,
                IsCompleted = x.IsCompleted
            }).ToList();
        }

        public async Task<ICollection<ToDoItemGetDto>> GetAllToDoItemsByUserIdAsync(long userId)
        {
            var items = await _toDoItemRepository.SelectAllToDoItemsByUserIdAsync(userId);
            return items.Select(item => new ToDoItemGetDto
            {
                ToDoItemId = item.ToDoItemId,
                Title = item.Title,
                Description = item.Description,
                DueDate = item.DueDate,
                IsCompleted = item.IsCompleted
            }).ToList();
        }

        public async Task<ICollection<ToDoItemGetDto>> GetByDueDateAsync(DateTime dueDate, long userId)
        {
            var items = await _toDoItemRepository.SelectByDueDateAsync(dueDate, userId);
            return items.Select(item => new ToDoItemGetDto
            {
                ToDoItemId = item.ToDoItemId,
                Title = item.Title,
                Description = item.Description,
                DueDate = item.DueDate,
                IsCompleted = item.IsCompleted
            }).ToList();
        }

        public async Task<ICollection<ToDoItemGetDto>> GetOverdueItemsAsync(long userId)
        {
            var items = await _toDoItemRepository.SelectOverdueItemsAsync(userId);
            return items.Select(item => new ToDoItemGetDto
            {
                ToDoItemId = item.ToDoItemId,
                Title = item.Title,
                Description = item.Description,
                DueDate = item.DueDate,
                IsCompleted = item.IsCompleted
            }).ToList();
        }

    }
}
