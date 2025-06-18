﻿using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IUserRepository
{
    Task<long> AddUserAsync(User user);
    Task<User> GetUserByIdAsync(long id);
    Task UpdateUser(User user);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserByUserNameAsync(string userName);
    Task UpdateUserRoleAsync(long userId, string userRole);
    Task DeleteUserByIdAsync(long userId);
    Task<bool> CheckUserById(long userId);
    Task<bool> CheckUsernameExists(string username);
    Task<long?> CheckEmailExistsAsync(string email);
    Task<bool> CheckPhoneNumberExists(string phoneNum);
    Task<List<User>> GetAllUsersAsync();
    Task SaveRefreshTokenAsync(long userId, string refreshToken);
    Task<User> GetByRefreshTokenAsync(string refreshToken);
    Task ReplaceRefreshTokenAsync(long userId, string newRefreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}