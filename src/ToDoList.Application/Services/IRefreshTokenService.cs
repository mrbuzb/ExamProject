using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services
{
    public interface IRefreshTokenService
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken, long userId);
        Task DeleteRefreshTokenAsync(string refreshToken);
    }
}