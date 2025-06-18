using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Repositories;

public class RequestLogRepository : IRequestLogRepository
{
    private readonly AppDbContext _context;

    public RequestLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(RequestLog log)
    {
        _context.RequestLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
