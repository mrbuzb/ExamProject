using MyProject.Extensions;
using ToDoList.Api.Configurations;
using ToDoList.Api.Endpoints;
using ToDoList.Api.Middlewares;

namespace ToDoList.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            ServiceCollectionExtensions.AddSwaggerWithJwt(builder.Services);

            builder.ConfigurationJwtAuth();
            builder.ConfigureDependicys();
            builder.ConfigureDataBase();
            builder.ConfigureSerilog();
            builder.ConfigureJwtSettings();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost5173", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            var app = builder.Build();
            app.UseCors("AllowLocalhost5173");
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRoleEndpoints();
            app.MapAdminEndpoints();
            app.MapAuthEndpoints();
            app.MapToDoItemEndpoints();

            //app.MapControllers();
            app.Run();
        }
    }

}
