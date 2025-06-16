using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext MainContext;

    public RefreshTokenRepository(AppDbContext mainContext)
    {
        MainContext = mainContext;
    }

    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        await MainContext.RefreshTokens.AddAsync(refreshToken);
        await MainContext.SaveChangesAsync();
    }

    public async Task DeleteRefreshToken(string refreshToken)
    {
        var token = await MainContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        
        if (token != null)
        {
            MainContext.RefreshTokens.Remove(token);
            await MainContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Refresh token not found.");
        }
    }

    public async Task<RefreshToken> SelectRefreshToken(string refreshToken, long userId)
    {
        var res= await MainContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);
        return res ?? throw new InvalidOperationException("Refresh token not found.");
    }
}
