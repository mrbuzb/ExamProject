using System.Security.Claims;
using ToDoList.Application.Dtos;

namespace ToDoList.Application.Helpers;

public interface ITokenService
{
    public string GenerateToken(UserGetDto user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}






