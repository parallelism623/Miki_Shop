using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.Interfaces
{
    public interface ICacheRepository
    {
        Task<string> GetCacheResponeAsync(string cacheKey);
        Task RemoveCacheResponeAsync(string cacheKey);
        Task SetCacheResponeAsync(string cacheKey, object respone, TimeSpan timeOut);
        IAsyncEnumerable<string> GetKeyAsync (string pattern);
    }
}
