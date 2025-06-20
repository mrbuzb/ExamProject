using FluentValidation;
using ToDoList.Application.Dtos;
using ToDoList.Application.Helpers;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Application.Validators;
using ToDoList.Infrastructure.Persistence.Repositories;

namespace ToDoList.Api.Configurations;

public static class DependicysConfigurations
{
    public static void ConfigureDependicys(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();

        builder.Services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
        builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
        builder.Services.AddScoped<IValidator<ToDoItemCreateDto>, ToDoItemCreateDtoValidator>();
        builder.Services.AddScoped<IValidator<ToDoItemUpdateDto>, ToDoItemUpdateDtoValidator>();
    }
}
