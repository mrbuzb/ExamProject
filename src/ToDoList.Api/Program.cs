using MyProject.Extensions;

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

            // 🔐 Auth va Swagger JWT extensionlar
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSwaggerWithJwt();

            builder.Services.AddDbContext<PostgresDbContext>(options =>
               options.UseNpgsql(builder.Configuration.GetConnectionString("NpgslConnection")));


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
