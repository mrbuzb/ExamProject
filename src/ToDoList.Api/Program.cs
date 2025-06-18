using Microsoft.EntityFrameworkCore;
using MyProject.Extensions;
using Serilog;
using Serilog.Events;
using ToDoList.Api.Middlewares;
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

            // Serilog konfiguratsiyasi
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // 🔁 appsettings.json dan o‘qiydi
                .Enrich.FromLogContext()
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            builder.Host.UseSerilog(); // Serilog’ni ulash

            // 📦 Service va Controller’lar
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // JWT + Swagger konfiguratsiya
            ServiceCollectionExtensions.AddSwaggerWithJwt(builder.Services);

            // 👨‍💻 Servis va repo implementatsiyalari
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // 📂 DB ulash
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<PostgresDbContext>(options =>
               options.UseNpgsql(builder.Configuration.GetConnectionString("NpgslConnection")));


            var app = builder.Build();

            // 🌐 Middleware loglash
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<SuccessRequestLoggingMiddleware>();

            // 📄 Swagger faqat dev uchun
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }

}
