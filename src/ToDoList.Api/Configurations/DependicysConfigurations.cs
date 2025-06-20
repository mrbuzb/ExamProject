using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Infrastructure.Persistence.Repositories;

namespace ToDoList.Api.Configurations;

public static class DependicysConfigurations
{
    public static void ConfigureDependicys(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
    }
}
