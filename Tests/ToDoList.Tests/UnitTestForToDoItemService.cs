using FluentValidation;
using FluentValidation.Results;
using Moq;
using ToDoList.Application.Dtos;
using ToDoList.Application.DTOs;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Core.Errors;
using ToDoList.Domain.Entities;

namespace ToDoList.Tests
{
    public class UnitTestForToDoItemService
    {
        private readonly Mock<IToDoItemRepository> _repoMock;
        private readonly Mock<IValidator<ToDoItemCreateDto>> _createValidatorMock;
        private readonly Mock<IValidator<ToDoItemUpdateDto>> _updateValidatorMock;
        private readonly ToDoItemService _service;

        public UnitTestForToDoItemService()
        {
            _repoMock = new Mock<IToDoItemRepository>();
            _createValidatorMock = new Mock<IValidator<ToDoItemCreateDto>>();
            _updateValidatorMock = new Mock<IValidator<ToDoItemUpdateDto>>();
            _service = new ToDoItemService(_repoMock.Object, _createValidatorMock.Object, _updateValidatorMock.Object);
        }

        [Fact]
        public async Task SearchToDoItemsAsync_ReturnsDtos()
        {
            var userId = 1L;
            var keyword = "test";
            _repoMock.Setup(r => r.SearchToDoItemsAsync(keyword, userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 1, Title = "A", Description = "B", DueDate = DateTime.Now, IsCompleted = false } });

            var result = await _service.SearchToDoItemsAsync(keyword, userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllToDoItemsByUserIdAsync_ReturnsDtos()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.SelectAllToDoItemsByUserIdAsync(userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 2, Title = "B", Description = "C", DueDate = DateTime.Now, IsCompleted = true } });

            var result = await _service.GetAllToDoItemsByUserIdAsync(userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByDueDateAsync_ReturnsDtos()
        {
            var userId = 1L;
            var dueDate = DateTime.Today;
            _repoMock.Setup(r => r.SelectByDueDateAsync(dueDate, userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 3, Title = "C", Description = "D", DueDate = dueDate, IsCompleted = false } });

            var result = await _service.GetByDueDateAsync(dueDate, userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetOverdueItemsAsync_ReturnsDtos()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.SelectOverdueItemsAsync(userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 4, Title = "D", Description = "E", DueDate = DateTime.Now.AddDays(-1), IsCompleted = false } });

            var result = await _service.GetOverdueItemsAsync(userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task InsertToDoItemAsync_ValidDto_ReturnsId()
        {
            var userId = 1L;
            var dto = new ToDoItemCreateDto();
            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _repoMock.Setup(r => r.InsertToDoItemAsync(It.IsAny<ToDoItem>()))
                .ReturnsAsync(42L);

            var result = await _service.InsertToDoItemAsync(dto, userId);

            Assert.Equal(42L, result);
        }

        [Fact]
        public async Task InsertToDoItemAsync_InvalidDto_ThrowsAuthException()
        {
            var userId = 1L;
            var dto = new ToDoItemCreateDto();
            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("Title", "Required") }));

            await Assert.ThrowsAsync<AuthException>(() => _service.InsertToDoItemAsync(dto, userId));
        }

        [Fact]
        public async Task DeleteToDoItemByIdAsync_CallsRepository()
        {
            var userId = 1L;
            var id = 5L;
            _repoMock.Setup(r => r.DeleteToDoItemByIdAsync(id, userId)).Returns(Task.CompletedTask);

            await _service.DeleteToDoItemByIdAsync(id, userId);

            _repoMock.Verify(r => r.DeleteToDoItemByIdAsync(id, userId), Times.Once);
        }

        [Fact]
        public async Task DeleteCompletedAsync_ReturnsCount()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.DeleteCompletedAsync(userId)).ReturnsAsync(3);

            var result = await _service.DeleteCompletedAsync(userId);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task UpdateToDoItemAsync_ValidDto_CallsRepository()
        {
            var userId = 1L;
            var dto = new ToDoItemUpdateDto();
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _repoMock.Setup(r => r.UpdateToDoItemAsync(It.IsAny<ToDoItem>(), userId)).Returns(Task.CompletedTask);

            await _service.UpdateToDoItemAsync(dto, userId);

            _repoMock.Verify(r => r.UpdateToDoItemAsync(It.IsAny<ToDoItem>(), userId), Times.Once);
        }

        [Fact]
        public async Task UpdateToDoItemAsync_InvalidDto_ThrowsAuthException()
        {
            var userId = 1L;
            var dto = new ToDoItemUpdateDto();
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("Title", "Required") }));

            await Assert.ThrowsAsync<AuthException>(() => _service.UpdateToDoItemAsync(dto, userId));
        }

        [Fact]
        public async Task SelectAllToDoItemsByUserIdAsync_ReturnsDtos()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.SelectAllToDoItemsByUserIdAsync(userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 6, Title = "E", Description = "F", DueDate = DateTime.Now, IsCompleted = false } });

            var result = await _service.SelectAllToDoItemsByUserIdAsync(userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task SelectToDoItemByIdAsync_ReturnsDto()
        {
            var userId = 1L;
            var id = 7L;
            _repoMock.Setup(r => r.SelectToDoItemByIdAsync(id, userId))
                .ReturnsAsync(new ToDoItem { ToDoItemId = id, Title = "F", Description = "G", DueDate = DateTime.Now, IsCompleted = false });

            var result = await _service.SelectToDoItemByIdAsync(id, userId);

            Assert.Equal(id, result.ToDoItemId);
        }

        [Fact]
        public async Task SelectByDueDateAsync_ReturnsDtos()
        {
            var userId = 1L;
            var dueDate = DateTime.Today;
            _repoMock.Setup(r => r.SelectByDueDateAsync(dueDate, userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 8, Title = "G", Description = "H", DueDate = dueDate, IsCompleted = false } });

            var result = await _service.SelectByDueDateAsync(dueDate, userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task SelectCompletedAsync_ReturnsDtos()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.SelectCompletedAsync(userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 9, Title = "H", Description = "I", DueDate = DateTime.Now, IsCompleted = true } });

            var result = await _service.SelectCompletedAsync(userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task SelectIncompletedAsync_ReturnsDtos()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.SelectIncompletedAsync(userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 10, Title = "I", Description = "J", DueDate = DateTime.Now, IsCompleted = false } });

            var result = await _service.SelectIncompletedAsync(userId);

            Assert.Single(result);
        }

        [Fact]
        public async Task SelectOverdueItemsAsync_ReturnsDtos()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.SelectOverdueItemsAsync(userId))
                .ReturnsAsync(new List<ToDoItem> { new() { ToDoItemId = 11, Title = "J", Description = "K", DueDate = DateTime.Now.AddDays(-2), IsCompleted = false } });

            var result = await _service.SelectOverdueItemsAsync(userId);

            Assert.Single(result);
        }

        [Fact]
        public void SelectTotalCountAsync_ReturnsCount()
        {
            var userId = 1L;
            _repoMock.Setup(r => r.SelectTotalCountAsync(userId)).Returns(5);

            var result = _service.SelectTotalCountAsync(userId);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task MarkAsCompletedAsync_CallsRepository()
        {
            var userId = 1L;
            var id = 12L;
            _repoMock.Setup(r => r.MarkAsCompletedAsync(id, userId)).Returns(Task.CompletedTask);

            await _service.MarkAsCompletedAsync(id, userId);

            _repoMock.Verify(r => r.MarkAsCompletedAsync(id, userId), Times.Once);
        }

        [Fact]
        public async Task SetDueDateAsync_CallsRepository()
        {
            var userId = 1L;
            var id = 13L;
            var dueDate = DateTime.Today;
            _repoMock.Setup(r => r.SetDueDateAsync(id, userId, dueDate)).Returns(Task.CompletedTask);

            await _service.SetDueDateAsync(id, userId, dueDate);

            _repoMock.Verify(r => r.SetDueDateAsync(id, userId, dueDate), Times.Once);
        }

        [Fact]
        public async Task GetSummaryAsync_ReturnsSummary()
        {
            var userId = 1L;
            var summary = new ToDoSummaryDto();
            _repoMock.Setup(r => r.GetSummaryAsync(userId)).ReturnsAsync(summary);

            var result = await _service.GetSummaryAsync(userId);

            Assert.Equal(summary, result);
        }
    }
}