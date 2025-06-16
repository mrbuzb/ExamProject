using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken), "Refresh token cannot be null.");
            }
            await _refreshTokenRepository.AddRefreshToken(refreshToken);
        }

        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));
            }
            await _refreshTokenRepository.DeleteRefreshToken(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken, long userId)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));
            }
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be a positive number.");
            }
            return await _refreshTokenRepository.SelectRefreshToken(refreshToken, userId);
        }
    }
}
