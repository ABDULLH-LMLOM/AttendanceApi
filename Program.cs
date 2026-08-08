using AttendanceApi.Data;
using AttendanceApi.Service;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();

            // HttpClient for ESP32
            builder.Services.AddHttpClient("Esp32Client", client =>
            {
                client.BaseAddress = new Uri("http://10.82.175.189");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddScoped<AttendanceService>();

            // Database
            var connectionString = builder.Configuration.GetConnectionString("PublicServer");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
                .ExcludeFromDescription();

            app.UseWebSockets();

            app.MapControllers();

            app.Run();
        }
    }
}