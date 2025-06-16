 namespace ToDoList.Application.Dtos;

public class GetAllResponseModel
{
    public int TotalCount { get; set; }

    public List<ToDoItemGetDto> ToDoItemGetDtos { get; set; }
}
