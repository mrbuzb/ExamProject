using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services;

public interface IAuthService
{
    Task<long> SignUpUserAsync(UserCreateDto dto);
    Task<LoginResponseDto> LoginUserAsync(UserLoginDto dto);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto dto);
    Task LogOutAsync(string refreshToken);
}
