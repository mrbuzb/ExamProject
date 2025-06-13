using ToDoList.Application.Dtos;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Serveces.Converters;

namespace ToDoList.Application.Serveces;

public class UserServices(IUserRepository _userRepo) : IUserServices
{
    public async Task<List<UserGetDto>> GetAllUsersAsync()
    {
        var users = await _userRepo.GetAllUsersAsync();
        return users.Select(user => Converter.UserGetDtoConverter(user)).ToList();
    }
}
