using ToDoList.Application.Dtos;

namespace ToDoList.Application.Serveces;

public interface IUserServices
{
    Task<List<UserGetDto>> GetAllUsersAsync();
}