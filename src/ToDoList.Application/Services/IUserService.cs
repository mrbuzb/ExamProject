using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(UserCreateDto dto);
    Task<User> GetUserByUserNameAsync(string username);
}
