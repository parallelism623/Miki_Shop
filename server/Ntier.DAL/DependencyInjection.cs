using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ntier.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Ntier.DAL.Interfaces;
using Ntier.DAL.Repositories;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;
namespace Ntier.DAL
{
    public static class DependencyInjection
    {
        public static void RegisterDALDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(typeof(ShopContext));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
        }
    }
}
