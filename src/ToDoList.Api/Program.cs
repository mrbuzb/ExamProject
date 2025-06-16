using Microsoft.EntityFrameworkCore;
using MyProject.Extensions;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Services;
using ToDoList.Infrastructure.Persistence;
using ToDoList.Infrastructure.Persistence.Repositories;

namespace ToDoList.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔧 Service konfiguratsiyalari
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // 🔐 JWT va Swagger konfiguratsiya
            ServiceCollectionExtensions.AddSwaggerWithJwt(builder.Services); // Explicitly specify the desired method
                                                                           
            // 📦 Servislar va repolar
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // 📂 DbContext konfiguratsiyasi
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // 🔧 Middlewarelar
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();   // ⚠️ Auth bo'lsa bu muhim
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
