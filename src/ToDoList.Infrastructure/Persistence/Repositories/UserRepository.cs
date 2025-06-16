using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<long> AddUserAync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceRefreshTokenAsync(long userId, string newRefreshToken)
    {
        throw new NotImplementedException();
    }

    public Task RevokeRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public Task<User> GetUserByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByIdAync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByUserNameAync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserRoleAsync(long userId, string userRole)
    {
        throw new NotImplementedException();
    }
}
