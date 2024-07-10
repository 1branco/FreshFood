using Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CoreDatabase")));

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}