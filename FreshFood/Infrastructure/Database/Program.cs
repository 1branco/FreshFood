using Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FreshFoodContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FreshFoodDatabase")));

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}