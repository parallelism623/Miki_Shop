using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Ntier.BLL.Configuration;
using Ntier.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.BLL.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _slidingExpireTime;
        public CacheAttribute(int slidingExpireTime)
        {
            _slidingExpireTime = slidingExpireTime;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<CacheSettings>();
            if (cacheSettings == null || cacheSettings.Enable == false)
            {
                await next();
                return;
            }
            var keyCache = await GetKeyCacheAsync(context.HttpContext.Request);
            var cacheRepository = context.HttpContext.RequestServices.GetService<ICacheRepository>();
            var respone = await cacheRepository.GetCacheResponeAsync(keyCache);
            if (!string.IsNullOrEmpty(respone))
            {
                var contentResult = new ContentResult
                {
                    Content = respone,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var excutedContext = await next();
            if (excutedContext.Result is OkObjectResult result)
            {
                await cacheRepository.SetCacheResponeAsync(keyCache, result.Value, TimeSpan.FromSeconds(_slidingExpireTime));
            }
            return;
        }
        private async Task<string> GetKeyCacheAsync(HttpRequest request)
        {
            var result = new StringBuilder();
            result.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                result.Append($"|{key}--{value}");
            }
            return result.ToString(); 
            
        }
    }
}
