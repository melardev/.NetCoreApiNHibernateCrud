using System.Threading.Tasks;
using ApiCoreNHibernateCrud.Data;
using ApiCoreNHibernateCrud.Seeds;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCoreNHibernateCrud
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var sessionFactoryBuilder = services.GetRequiredService<ISessionFactoryBuilder>();
                await DbSeeder.Seed(sessionFactoryBuilder);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:8080", "https://localhost:5001")
                .UseStartup<Startup>();
        }
    }
}