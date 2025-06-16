using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Entities;

public class RequestLog
{
    public int Id { get; set; }
    public string? Method { get; set; }
    public string? Path { get; set; }
    public string? Controller { get; set; }
    public string? Action { get; set; }
    public string? RequestBody { get; set; }
    public int StatusCode { get; set; }
    public long ResponseTimeMs { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

