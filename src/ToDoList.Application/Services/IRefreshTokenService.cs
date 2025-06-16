using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public interface IRefreshTokenService
{
    Task<RefreshToken> CreateRefreshTokenAsync(long userId);
    Task<RefreshToken> GetByTokenAsync(string token);
    Task RevokeTokenAsync(string token);
}
