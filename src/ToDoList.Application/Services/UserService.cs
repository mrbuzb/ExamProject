using ToDoList.Application.Interfaces;
using ToDoList.Core.Errors;

namespace ToDoList.Application.Services;

public class UserService(IUserRepository _userRepository) : IUserService
{
    public async Task DeleteUserByIdAsync(long userId, string userRole)
    {
        if (userRole == "SuperAdmin")
        {
            await _userRepository.DeleteUserByIdAsync(userId);
        }
        else if (userRole == "Admin")
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user.Role.Name == "User")
            {
                await _userRepository.DeleteUserByIdAsync(userId);
            }
            else
            {
                throw new NotAllowedException("Admin can not delete Admin or SuperAdmin");
            }
        }
    }

    public async Task UpdateUserRoleAsync(long userId, string userRole) => await _userRepository.UpdateUserRoleAsync(userId, userRole);
}
