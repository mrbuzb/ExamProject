namespace ToDoList.Application.Dtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Username { get; set; }
}
