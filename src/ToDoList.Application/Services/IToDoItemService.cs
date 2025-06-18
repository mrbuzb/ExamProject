using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services
{
    public interface IToDoItemService
    {
        Task<ICollection<ToDoItemGetDto>> SearchToDoItemsAsync(string keyword);
    }
}
