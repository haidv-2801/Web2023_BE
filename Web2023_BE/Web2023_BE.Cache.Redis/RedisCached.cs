
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace Web2023_BE.Cache.Redis
{
    public class RedisCached : BaseCached, IDistCached
    {
        private const string TAG = "RedisCached";
        private readonly IDistributedCache _distributedCache;
        private readonly static Type STRING_TYPE = typeof(string);

        public RedisCached(
            IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        protected override T GetCache<T>(string key)
        {
            string jsonText = "";
            //try
            //{
            //Lấy giá trị trong cached
            jsonText = _distributedCache.GetString(key);

            //Kiểm tra xem có giá trị hay không?
            if (string.IsNullOrEmpty(jsonText)) return default;

            if (typeof(T) == STRING_TYPE)
            {
                return (T)(object)jsonText;
            }

            //parse về đùng kiểu output
            return JsonConvert.DeserializeObject<T>(jsonText);
            //}
            //catch (Exception ex)
            //{
            //    _log.LogError(ex, $"{TAG} {ex.Message} {Environment.NewLine}. Key: {key}, cacheKey: {key}, jsonText: {jsonText}");
            //    throw;
            //}
        }

        protected override void SetCache<T>(string key, T data, TimeSpan expired)
        {
            string jsonText = "";

            //Convert dữ liệu từ đối tượng sang chuỗi
            if (data is string)
            {
                jsonText = (string)(object)data;
            }
            else
            {
                jsonText = JsonConvert.SerializeObject(data);
            }

            _distributedCache.SetString(key, jsonText, GetDistributedCacheEntryOptions(expired));
            //}
            //catch (Exception ex)
            //{
            //    _log.LogError(ex, $"{TAG} {ex.Message} {Environment.NewLine}. Key: {key}, cacheKey: {key}, jsonText: {jsonText}");
            //    throw;
            //}
        }

        protected override void RemoveCache(string key)
        {
            //try
            //{
            _distributedCache.Remove(key);
            //}
            //catch (Exception ex)
            //{
            //    _log.LogError(ex, $"{TAG} {ex.Message} {Environment.NewLine}. Key: {key}, cacheKey: {key}");
            //    throw;
            //}
        }

        /// <summary>
        /// Cấu hình thời gian hết hạn của Cached
        /// </summary>
        /// <param name="expired">Thời gian lưu cache (tính theo giây)</param>
        private DistributedCacheEntryOptions GetDistributedCacheEntryOptions(TimeSpan expired)
        {
            return new DistributedCacheEntryOptions().SetAbsoluteExpiration(expired);
        }
    }
}
