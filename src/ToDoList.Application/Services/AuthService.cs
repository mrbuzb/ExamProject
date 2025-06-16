using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoList.Application.Dtos;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Domain.Entities;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<long> SignUpUserAsync(UserCreateDto dto)
    {
        if (await _userRepo.ExistsByUsername(dto.UserName))
            throw new Exception("Bu username band");

        var salt = Guid.NewGuid().ToString();
        var passwordHash = HashPassword(dto.Password, salt);

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Password = passwordHash,
            Salt = salt,
            RoleId = 1 // oddiy user
        };

        return await _userRepo.CreateUserAsync(user);
    }

    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto dto)
    {
        var user = await _userRepo.GetByUsernameAsync(dto.UserName);
        if (user == null)
            throw new Exception("Foydalanuvchi topilmadi");

        var hash = HashPassword(dto.Password, user.Salt);
        if (hash != user.Password)
            throw new Exception("Parol noto'g'ri");

        var accessToken = GenerateJwtToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        await _userRepo.SaveRefreshTokenAsync(user.UserId, refreshToken);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.UserName
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        var user = await _userRepo.GetByRefreshTokenAsync(request.RefreshToken);
        if (user == null)
            throw new Exception("Token noto'g'ri yoki tugagan");

        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = Guid.NewGuid().ToString();

        await _userRepo.ReplaceRefreshTokenAsync(user.UserId, newRefreshToken);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Username = user.UserName
        };
    }

    public async Task LogOutAsync(string token)
    {
        await _userRepo.RevokeRefreshTokenAsync(token);
    }

    private string HashPassword(string password, string salt)
    {
        var sha = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("role", user.Role?.Name ?? "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(20),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
