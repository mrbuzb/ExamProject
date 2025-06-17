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

    public async Task<long> AddUserAync(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email || u.UserName == user.UserName);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with the same email or username already exists.");
        }

        var addedUser = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return addedUser.Entity.UserId;
    }

    public async Task<long?> CheckEmailExistsAsync(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        return user?.UserId;
    }

    public async Task<bool> CheckPhoneNumberExists(string phoneNum)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNum);
        return user != null;
    }

    public async Task<bool> CheckUserById(long userId)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.UserId == userId);
    }

    public async Task<bool> CheckUsernameExists(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.UserName == username);
    }

    public async Task DeleteUserByIdAsync(long userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User> GetByRefreshTokenAsync(string refreshToken)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with refresh token '{refreshToken}' not found.");
        }
        
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with email '{email}' not found.");
        }
        
        return user;
    }

    public async Task<User> GetUserByIdAync(long id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == id);
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {id} not found.");
        }
        
        return user;
    }

    public async Task<User> GetUserByUserNameAync(string userName)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == userName);
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with username '{userName}' not found.");
        }
        
        return user;
    }

    public async Task ReplaceRefreshTokenAsync(long userId, string newRefreshToken)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }
        var existingToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == newRefreshToken);
        if (existingToken != null)
        {
            existingToken.IsRevoked = false;
            existingToken.Expires = DateTime.UtcNow.AddDays(30); // Example expiration time
        }
        else
        {
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(30), // Example expiration time
                IsRevoked = false
            });
        }
        await _context.SaveChangesAsync();
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        
        if (token == null)
        {
            throw new InvalidOperationException($"Refresh token '{refreshToken}' not found.");
        }
        
        token.IsRevoked = true;
        await _context.SaveChangesAsync();
    }

    public async Task SaveRefreshTokenAsync(long userId, string refreshToken)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }
        
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(30), // Example expiration time
            IsRevoked = false
        });
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.UserId);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with ID {user.UserId} not found.");
        }
        existingUser.Email = user.Email;
        existingUser.UserName = user.UserName;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Password = user.Password; // Consider hashing the password before saving
        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserRoleAsync(long userId, string userRoleName)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        var role = await _context.UserRoles
            .FirstOrDefaultAsync(r => r.Name == userRoleName);
        if (role == null)
        {
            throw new InvalidOperationException($"Role with name '{userRoleName}' not found.");
        }

        user.Role = role;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
