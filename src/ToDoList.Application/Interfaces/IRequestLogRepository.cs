using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces
{
    public interface IRequestLogRepository
    {
        Task LogAsync(RequestLog log);
    }
}
