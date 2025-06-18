namespace ToDoList.Application.DTOs;

public class ToDoSummaryDto
{
    public int Total { get; set; }
    public int Completed { get; set; }
    public int Incompleted { get; set; }
    public int Overdue { get; set; }
}
