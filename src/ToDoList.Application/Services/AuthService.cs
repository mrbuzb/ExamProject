using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;
using ToDoList.Domain.Entities;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthService(IUserService userService, IRefreshTokenService refreshTokenService)
    {
        _userService = userService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<long> SignUpUserAsync(UserCreateDto dto)
    {
        var user = await _userService.CreateUserAsync(dto);
        return user.UserId;
    }

    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto dto)
    {
        var user = await _userService.GetUserByUserNameAsync(dto.UserName);
        if (user == null || user.Password != dto.Password)
            throw new UnauthorizedAccessException("Invalid credentials");

        var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user.UserId);

        return new LoginResponseDto
        {
            AccessToken = Guid.NewGuid().ToString(),
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto dto)
    {
        var oldToken = await _refreshTokenService.GetByTokenAsync(dto.Token);
        if (oldToken == null || oldToken.IsRevoked || oldToken.Expires < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Token yaroqsiz");

        var newToken = await _refreshTokenService.CreateRefreshTokenAsync(oldToken.UserId);
        await _refreshTokenService.RevokeTokenAsync(dto.Token);

        return new LoginResponseDto
        {
            AccessToken = Guid.NewGuid().ToString(),
            RefreshToken = newToken.Token
        };
    }

    public async Task LogOutAsync(string refreshToken)
    {
        await _refreshTokenService.RevokeTokenAsync(refreshToken);
    }

    private string GenerateJwtToken(User user, IConfiguration config)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
