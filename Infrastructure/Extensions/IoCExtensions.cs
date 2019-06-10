using ApiCoreNHibernateCrud.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiCoreNHibernateCrud.Infrastructure.Extensions
{
    public static class IoCExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<TodoService>();
        }
    }
}