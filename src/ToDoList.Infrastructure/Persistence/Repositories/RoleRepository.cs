using ToDoList.Application.Interfaces;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task<List<UserRole>> GetAllRolesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<User>> GetAllUsersByRoleAsync(string role)
    {
        throw new NotImplementedException();
    }

    public Task<long> GetRoleIdAsync(string role)
    {
        throw new NotImplementedException();
    }
}
