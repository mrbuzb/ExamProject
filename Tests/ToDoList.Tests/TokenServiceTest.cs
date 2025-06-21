using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoList.Application.Dtos;
using ToDoList.Application.Helpers;
using ToDoList.Application.Settings;

namespace ToDoList.Tests;

public class TokenServiceTest
{
    private readonly JwtAppSettings _jwtSettings;
    private readonly TokenService _tokenService;

    public TokenServiceTest()
    {
        _jwtSettings = new JwtAppSettings(
            issuer: "TestIssuer",
            audience: "TestAudience",
            securityKey: "ThisIsASecretKeyForTestingPurposes123!",
            lifetime: "1"
        );
        _tokenService = new TokenService(_jwtSettings);
    }

    private UserGetDto GetTestUser()
    {
        return new UserGetDto
        {
            UserId = 1,
            FirstName = "John",
            LastName = "Doe",
            UserName = "johndoe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Salt = "somesalt",
            Role = "Admin"
        };
    }

    [Fact]
    public void GenerateToken_ShouldReturn_ValidJwtToken()
    {
        // Arrange
        var user = GetTestUser();

        // Act
        var token = _tokenService.GenerateToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
        Assert.Equal(_jwtSettings.Audience, jwtToken.Audiences.First());
        Assert.Contains(jwtToken.Claims, c => c.Type == "UserId" && c.Value == user.UserId.ToString());
        Assert.Contains(jwtToken.Claims, c => c.Type == "FirstName" && c.Value == user.FirstName);
        Assert.Contains(jwtToken.Claims, c => c.Type == "LastName" && c.Value == user.LastName);
        Assert.Contains(jwtToken.Claims, c => c.Type == "PhoneNumber" && c.Value == user.PhoneNumber);
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.UniqueName && c.Value == user.UserName);
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == user.Role);
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Email && c.Value == user.Email);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturn_Base64String()
    {
        // Act
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(refreshToken));
        // Should be 64 bytes, base64 encoded
        var bytes = Convert.FromBase64String(refreshToken);
        Assert.Equal(64, bytes.Length);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ShouldThrowException_ForInvalidToken()
    {
        // Arrange
        var invalidToken = "invalid.token.value";

        // Act & Assert
        Assert.ThrowsAny<Exception>(() =>
        {
            _tokenService.GetPrincipalFromExpiredToken(invalidToken);
        });
    }
}