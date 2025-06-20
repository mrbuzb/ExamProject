using System.Security.Claims;
using FluentValidation;
using ToDoList.Application.Dtos;
using ToDoList.Application.Helpers;
using ToDoList.Application.Helpers.Security;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Application.Settings;
using ToDoList.Core.Errors;
using ToDoList.Domain.Entities;

public class AuthService(IRoleRepository _roleRepo, IValidator<UserCreateDto> _validator,
    IUserRepository _userRepo, ITokenService _tokenService,
    JwtAppSettings _jwtSetting, IValidator<UserLoginDto> _validatorForLogin,
    IRefreshTokenRepository _refTokRepo) : IAuthService
{
    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var validatorResult = await _validator.ValidateAsync(userCreateDto);
        if (!validatorResult.IsValid)
        {
            string errorMessages = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
            throw new AuthException(errorMessages);
        }

        var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

        var user = new User()
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            UserName = userCreateDto.UserName,
            PhoneNumber = userCreateDto.PhoneNumber,
            Password = tupleFromHasher.Hash,
            Salt = tupleFromHasher.Salt,
            Email = userCreateDto.Email,
            RoleId = await _roleRepo.GetRoleIdAsync("User")
        };

        long userId = await _userRepo.AddUserAync(user);



        var foundUser = await _userRepo.GetUserByIdAync(userId);


        await _userRepo.UpdateUser(foundUser);

        return userId;
    }


    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        var resultOfValidator = _validatorForLogin.Validate(userLoginDto);
        if (!resultOfValidator.IsValid)
        {
            string errorMessages = string.Join("; ", resultOfValidator.Errors.Select(e => e.ErrorMessage));
            throw new AuthException(errorMessages);
        }

        var user = await _userRepo.GetUserByUserNameAync(userLoginDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);

        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("UserName or password incorrect");
        }

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.Name,
            Email = user.Email,
        };

        var token = _tokenService.GenerateToken(userGetDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refTokRepo.CreateAsync(refreshTokenToDB);

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Expires = 24
        };


        return loginResponseDto;
    }


    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");


        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);


        var refreshToken = await _refTokRepo.GetByTokenAsync(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;

        var user = await _userRepo.GetUserByIdAync(userId);

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.Name,
        };

        var newAccessToken = _tokenService.GenerateToken(userGetDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refTokRepo.CreateAsync(refreshTokenToDB);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            Expires = 24
        };
    }

    public async Task LogOutAsync(string token) => await _refTokRepo.DeleteRefreshToken(token);


}
