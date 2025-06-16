using System.Security.Cryptography;
using System.Text;
using ToDoList.Application.Dtos;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User> CreateUserAsync(UserCreateDto dto)
    {
        var salt = GenerateSalt();
        var hashedPassword = HashPassword(dto.Password, salt);

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Password = hashedPassword,
            Salt = salt,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            RoleId = dto.RoleId,

        };

        var userId = await _userRepo.AddUserAync(user);
        user.UserId = userId;
        return user;
    }

    public async Task<User> GetUserByUserNameAsync(string username)
    {
        return await _userRepo.GetUserByUserNameAync(username);
    }

    private string GenerateSalt()
    {
        return Guid.NewGuid().ToString();
    }

    private string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }
}
