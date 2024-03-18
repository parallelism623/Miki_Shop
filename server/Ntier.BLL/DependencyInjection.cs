using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ntier.BLL.Configuration;
using Ntier.BLL.Extentions;
using Ntier.BLL.Interfaces;
using Ntier.BLL.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.BLL
{
    public static class DependencyInjection
    {
        public static void RegisterBLLDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, AuthorizationHandlerlExtensions>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper(typeof(Ntier.BLL.Extentions.AutoMapper).Assembly);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IImageService, ImageService>();
            var redisSettings = new CacheSettings();
            configuration.GetSection("RedisConfiguration").Bind(redisSettings);
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisSettings.ConnectionString));
            services.AddStackExchangeRedisCache(options => options.Configuration = redisSettings.ConnectionString);
            services.AddSingleton(redisSettings);
        }
    }
}
