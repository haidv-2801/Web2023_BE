using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.Cache;

namespace Web2023_BE.HostBase
{
    public class MicrosoftMemCached : BaseCached, IMemCached
    {
        #region "Variable"
        private readonly IMemoryCache _memoryCache;
        #endregion

        #region "Constructor"
        public MicrosoftMemCached(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        #endregion

        #region Method

        protected override T GetCache<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        protected override void SetCache<T>(string key, T data, TimeSpan expired)
        {
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions();

            // Keep in cache for this time, reset time if accessed.
            //cacheEntryOptions.SetSlidingExpiration(expired.Value);
            cacheEntryOptions.SetAbsoluteExpiration(expired);

            // Save data in cache.
            _memoryCache.Set(key, data, cacheEntryOptions);
        }

        protected override void RemoveCache(string key)
        {
            _memoryCache.Remove(key);
        }

        #endregion
    }
}
