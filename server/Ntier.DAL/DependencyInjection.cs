using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ntier.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Ntier.DAL.Interfaces;
using Ntier.DAL.Repositories;
namespace Ntier.DAL
{
    public static class DependencyInjection
    {
        public static void RegisterDALDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ShopContext));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
        }
    }
}
