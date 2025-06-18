using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;
using ToDoList.Application.Dtos;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services.Converters;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    //private readonly IMemoryCache _cache;
    //private const string CacheKey = "AllUsersCacheKey";
    public UserService(IUserRepository userRepo/*,IMemoryCache cache*/)
    {
        _userRepo = userRepo;
        //_cache = cache;

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

            var userId = await _userRepo.AddUserAsync(user);
            user.UserId = userId;
            return user;
        }

    public async Task<List<UserGetDto>> GetAllUsersAsync()
    {
        //if (_cache.TryGetValue(CacheKey, out List<UserGetDto> cachedUsers))
        //{
        //    return cachedUsers;
        //}

        var users = await _userRepo.GetAllUsersAsync();

        var userDtos = users
            .Select(user => Converter.UserGetDtoConverter(user))
            .ToList();

        //_cache.Set(CacheKey, userDtos, new MemoryCacheEntryOptions
        //{
        //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        //});

        return userDtos;
    }


    public async Task<User> GetUserByUserNameAsync(string username)
    {
        return await _userRepo.GetUserByUserNameAsync(username);
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

    public async Task UpdateAsync(UserGetDto dto)
    {
        var user = await _userRepo.GetUserByIdAsync(dto.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");
        }
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.UserName = dto.UserName;
        user.PhoneNumber = dto.PhoneNumber;
        user.Email = dto.Email;

        await _userRepo.UpdateUser(user); 
        //_cache.Remove(CacheKey);
    }

    public async Task<User> GetUserByIdAsync(long userId)
    {
        var user = await _userRepo.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
        return user;
    }

}
