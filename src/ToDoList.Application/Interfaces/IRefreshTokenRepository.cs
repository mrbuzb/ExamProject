using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken token);
    Task<RefreshToken> GetByTokenAsync(string token, long userId);
    Task DeleteRefreshToken(string refreshToken);
}
