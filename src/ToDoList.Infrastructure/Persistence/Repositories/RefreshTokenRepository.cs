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
    public Task AddRefreshToken(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken> SelectRefreshToken(string refreshToken, long userId)
    {
        throw new NotImplementedException();
    }
}
