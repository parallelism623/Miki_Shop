using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ntier.DAL.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public CacheRepository(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task<string?> GetCacheResponeAsync(string cacheKey)
        {
            if (cacheKey == null)
            {
                return null;
            }
            var respone = await _distributedCache.GetStringAsync(cacheKey);
            return respone ?? null;
        }

        private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
        {
            foreach(var ep in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(ep);
                foreach (var ser in server.Keys(pattern: pattern + "*"))
                {
                   yield return ser.ToString();
                }
            }
        }

        public async Task RemoveCacheResponeAsync(string cacheKey)
        {
            if (cacheKey == null) 
            {
                return;
            }
            var listKey = GetKeyAsync(cacheKey);
            await foreach(var key in listKey)
            {
                await _distributedCache.RemoveAsync(key); 
            }
        }

        public async Task SetCacheResponeAsync(string cacheKey, object respone, TimeSpan timeOut)
        {
            if (respone == null)
            {
                return;
            }
            var serializeJson = JsonConvert.SerializeObject(respone, new JsonSerializerSettings()
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            await _distributedCache.SetStringAsync(cacheKey, serializeJson, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut,
            });
        }
    }
}
