using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Serveces.Converters;

public static class Converter
{
    public static ToDoItemGetDto ToDoItemGetDtoConverter(ToDoItem toDoItem)
    {
        return new ToDoItemGetDto
        {
            ToDoItemId = toDoItem.ToDoItemId,
            Title = toDoItem.Title,
            Description = toDoItem.Description,
            IsCompleted = toDoItem.IsCompleted,
            CreatedAt = toDoItem.CreatedAt,
            DueDate = toDoItem.DueDate
        };
    }
    public static UserGetDto UserGetDtoConverter(User user)
    {
        return new UserGetDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            Salt = user.Salt,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.Name,
        };
    }


}
