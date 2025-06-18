using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<List<UserRole>> GetAllRolesAsync()
    {
        return await _context.UserRoles.ToListAsync()
            ?? throw new InvalidOperationException("No roles found in the database.");
    }




    public async Task<ICollection<User>> GetAllUsersByRoleAsync(string role)
    {
        var roleEntity = await _context.UserRoles
            .FirstOrDefaultAsync(r => r.Name.Equals(role, StringComparison.OrdinalIgnoreCase));
        if (roleEntity == null)
        {
            throw new InvalidOperationException($"Role '{role}' not found.");
        }
        return await _context.Users
            .Where(u => u.RoleId == roleEntity.Id)
            .ToListAsync() 
            ?? throw new InvalidOperationException("No users found for the specified role.");
    }

    public async Task<long> GetRoleIdAsync(string role)
    {
        var roleEntity = await _context.UserRoles
            .FirstOrDefaultAsync(r => r.Name.Equals(role, StringComparison.OrdinalIgnoreCase));
        if (roleEntity == null)
        {
            throw new InvalidOperationException($"Role '{role}' not found.");
        }
        return roleEntity.Id;
    }
}
