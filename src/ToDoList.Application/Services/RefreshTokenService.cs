using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Application.Dtos;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshRepo;

    public RefreshTokenService(IRefreshTokenRepository refreshRepo)
    {
        _refreshRepo = refreshRepo;
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(long userId)
    {
        var token = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        return await _refreshRepo.CreateAsync(token);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await _refreshRepo.GetByTokenAsync(token);
    }

    public async Task RevokeTokenAsync(string token)
    {
        var existing = await _refreshRepo.GetByTokenAsync(token);
        if (existing != null)
        {
            existing.IsRevoked = true;
            await _refreshRepo.SaveChangesAsync();
        }
    }
}
