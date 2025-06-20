using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ToDoList.Application.Dtos;
using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services.Converters;
using ToDoList.Core.Errors;

namespace ToDoList.Application.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly IValidator<ToDoItemCreateDto> _createDtoValidator;
        private readonly IValidator<ToDoItemUpdateDto> _updateValidator;

        public ToDoItemService(IToDoItemRepository toDoItemRepository, IValidator<ToDoItemCreateDto> createDtoValidator, IValidator<ToDoItemUpdateDto> updateValidator)
        {
            _toDoItemRepository = toDoItemRepository;
            _createDtoValidator = createDtoValidator;
            _updateValidator = updateValidator;
        }
        public async Task<ICollection<ToDoItemGetDto>> SearchToDoItemsAsync(string keyword,long userId)
        {
            var items = await _toDoItemRepository.SearchToDoItemsAsync(keyword,userId);

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

        public async Task<long> InsertToDoItemAsync(ToDoItemCreateDto dto,long userId)
        {
            var validatorResult = await _createDtoValidator.ValidateAsync(dto);
            if (!validatorResult.IsValid)
            {
                string errorMessages = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
                throw new AuthException(errorMessages);
            }
            var entity = Converter.ToDoItemGetDtoConverter(dto);
            entity.UserId = userId;
            return await _toDoItemRepository.InsertToDoItemAsync(entity);
        }

        public async Task DeleteToDoItemByIdAsync(long id, long userId)
        {
            await _toDoItemRepository.DeleteToDoItemByIdAsync(id,userId);
        }

        public async Task<int> DeleteCompletedAsync(long userId)
        {
            return await _toDoItemRepository.DeleteCompletedAsync(userId);
        }

        public async Task UpdateToDoItemAsync(ToDoItemUpdateDto toDoItem,long userId)
        {
            var validatorResult = await _updateValidator.ValidateAsync(toDoItem);
            if (!validatorResult.IsValid)
            {
                string errorMessages = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
                throw new AuthException(errorMessages);
            }
            var entity = Converter.ToDoItemGetDtoConverter(toDoItem);
            await _toDoItemRepository.UpdateToDoItemAsync(entity,userId);
        }

        public async Task<ICollection<ToDoItemGetDto>> SelectAllToDoItemsByUserIdAsync(long userId)
        {
            var entitys =await _toDoItemRepository.SelectAllToDoItemsByUserIdAsync(userId);
            return entitys.Select(Converter.ToDoItemGetDtoConverter).ToList();
        }

        public async Task<ToDoItemGetDto> SelectToDoItemByIdAsync(long id, long userId)
        {
            var entity =await _toDoItemRepository.SelectToDoItemByIdAsync(id,userId);
            return Converter.ToDoItemGetDtoConverter(entity);
        }

        public async Task<ICollection<ToDoItemGetDto>> SelectByDueDateAsync(DateTime dueDate, long userId)
        {
            var entity =await _toDoItemRepository.SelectByDueDateAsync(dueDate,userId);
            return entity.Select(Converter.ToDoItemGetDtoConverter).ToList() ;
        }

        public async Task<ICollection<ToDoItemGetDto>> SelectCompletedAsync(long userId)
        {
            var entitys =await _toDoItemRepository.SelectCompletedAsync(userId);
            return entitys.Select(Converter.ToDoItemGetDtoConverter).ToList();
        }

        public async Task<ICollection<ToDoItemGetDto>> SelectIncompletedAsync(long userId)     
        {
            var entitys = await _toDoItemRepository.SelectIncompletedAsync(userId);
            return entitys.Select(Converter.ToDoItemGetDtoConverter).ToList();
        }

        public async Task<ICollection<ToDoItemGetDto>> SelectOverdueItemsAsync(long userId)
        {
            var entitys = await _toDoItemRepository.SelectOverdueItemsAsync(userId);
            return entitys.Select(Converter.ToDoItemGetDtoConverter).ToList();
        }

        public int SelectTotalCountAsync(long userId)
        {
            return  _toDoItemRepository.SelectTotalCountAsync(userId);
        }

        public async Task MarkAsCompletedAsync(long id, long userId)
        {
            await _toDoItemRepository.MarkAsCompletedAsync(id, userId);
        }

        public async Task SetDueDateAsync(long id, long userId, DateTime dueDate)
        {
            await _toDoItemRepository.SetDueDateAsync(id, userId, dueDate);
        }

        public async Task<ToDoSummaryDto> GetSummaryAsync(long userId)
        {
            return await _toDoItemRepository.GetSummaryAsync(userId);
        }
    }
}
