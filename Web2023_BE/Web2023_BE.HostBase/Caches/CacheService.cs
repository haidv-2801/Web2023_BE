


using System.Text.RegularExpressions;
using Web2023_BE.Cache;
using Web2023_BE.Cache.Constants;


namespace Web2023_BE.Cache
{
    public class CacheService : ICacheService
    {
        private readonly CacheConfig _cacheConfig;
        private readonly IMemCached _memCached;
        private readonly Dictionary<string, IDistCached> _distCaches;
        private const string USER_ID = "{uid}";
        private const string DATABASE_ID = "{dbid}";
        private const string SESSSION_ID = "{sid}";
        private const string GUID = "{guid}";
        private const string CUSTOM = "{custom}";

        public CacheService(
            CacheConfig cacheConfig,
            IMemCached memCached,
            Dictionary<string, IDistCached> distCaches
            )
        {
            _cacheConfig = cacheConfig;
            _memCached = memCached;
            _distCaches = distCaches;
        }

        public T Get<T>(CacheParam param, bool removeAfterGet = false)
        {
            var itemConfig = this.GetItemConfig(param.Name);
            var cacheKey = this.GetCacheKey(itemConfig, param);

            CacheData<T> cacheData = null;

            //Nếu có cấu hình mem thì ưu tiên đọc mem trước
            if (itemConfig.MemSeconds > 0)
            {
                cacheData = _memCached.Get<CacheData<T>>(cacheKey, removeAfterGet);
            }

            //Có cấu hình dist thì đọc dist
            if (cacheData == null)
            {
                if (itemConfig.DistSeconds > 0)
                {
                    var distCache = this.GetDistCached(itemConfig);
                    cacheData = distCache.Get<CacheData<T>>(cacheKey, removeAfterGet);

                    //Nếu dist có mà mem không thì dí vào mem để lần sau đọc từ mem ra cho nhanh
                    if (cacheData != null && itemConfig.MemSeconds > 0 && !removeAfterGet)
                    {
                        _memCached.Set(cacheKey, cacheData, this.GetExpired(itemConfig.MemSeconds.Value));
                    }
                }
            }

            //nếu có cache thì trả về
            if (cacheData != null)
            {
                return cacheData.Value;
            }

            //nếu không thì trả về default value của kiểu dữ liệu
            return default(T);
        }

        public void Remove(CacheParam param)
        {
            var itemConfig = this.GetItemConfig(param.Name);
            var cacheKey = this.GetCacheKey(itemConfig, param);

            if (itemConfig.MemSeconds > 0)
            {
                _memCached.Remove(cacheKey);
            }

            if (itemConfig.DistSeconds > 0)
            {
                var distCache = this.GetDistCached(itemConfig);
                distCache.Remove(cacheKey);
            }
        }

        public int Set<T>(CacheParam param, T data)
        {
            var itemConfig = this.GetItemConfig(param.Name);
            var cacheKey = this.GetCacheKey(itemConfig, param);
            var cacheData = this.CreateCacheObject(data);
            int result = 0;

            if (itemConfig.DistSeconds > 0)
            {
                var distCache = this.GetDistCached(itemConfig);
                distCache.Set(cacheKey, cacheData, this.GetExpired(itemConfig.DistSeconds.Value));
                result = itemConfig.DistSeconds.Value;
            }
            else if (itemConfig.MemSeconds > 0)
            {
                _memCached.Set(cacheKey, cacheData, this.GetExpired(itemConfig.MemSeconds.Value));
                result = itemConfig.MemSeconds.Value;
            }

            return result;
        }

        /// <summary>
        /// Tạo đối tượng lưu cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="data">Dữ liệu</param>
        private CacheData<T> CreateCacheObject<T>(T data)
        {
            var obj = new CacheData<T>
            {
                Value = data
            };
            return obj;
        }

        /// <summary>
        /// Lấy thời gian hết hạn của cache
        /// </summary>
        private TimeSpan GetExpired(int expiredSeconds)
        {
            return TimeSpan.FromSeconds(expiredSeconds);
        }

        /// <summary>
        /// Lấy instance distcache theo config của cache item
        /// </summary>
        private IDistCached GetDistCached(CacheItemConfig itemConfig)
        {
            var cacheKey = string.IsNullOrEmpty(itemConfig.DistGroup) ? _cacheConfig.DistGroups[0] : itemConfig.DistGroup;
            if (!_distCaches.ContainsKey(cacheKey))
            {
                throw new KeyNotFoundException($"Thiếu cấu hình dist cache cacheKey {cacheKey}");
            }

            return _distCaches[cacheKey];
        }

        /// <summary>
        /// Lấy cacheKey cache
        /// </summary>
        public string GetCacheKey(CacheItemConfig config, CacheParam param)
        {
            var cacheKey = config.Key;
            var regex = new Regex(@"{(\w+)}");
            var rgMatch = regex.Matches(config.Key);
            if (rgMatch.Count > 0)
            {
                var invalidFields = new List<string>();
                for (var i = 0; i < rgMatch.Count; i++)
                {
                    var item = rgMatch[i].Value;
                    switch (item)
                    {
                        case USER_ID:
                            if (param.UserId != null)
                            {
                                cacheKey = cacheKey.Replace(USER_ID, param.UserId.ToString());
                            }
                            else
                            {
                                invalidFields.Add(item);
                            }
                            break;
                     
                        case SESSSION_ID:
                            if (param.SessionId != null)
                            {
                                cacheKey = cacheKey.Replace(SESSSION_ID, param.SessionId.ToString());
                            }
                            else
                            {
                                invalidFields.Add(item);
                            }
                            break;
                        case GUID:
                            if (param.Guid != null)
                            {
                                cacheKey = cacheKey.Replace(GUID, param.Guid.ToString());
                            }
                            else
                            {
                                invalidFields.Add(item);
                            }
                            break;
                        case CUSTOM:
                            if (param.Custom != null)
                            {
                                cacheKey = cacheKey.Replace(CUSTOM, param.Custom.ToString());
                            }
                            else
                            {
                                invalidFields.Add(item);
                            }
                            break;
                    }
                }

                if (invalidFields.Count > 0)
                {
                    throw new Exception($"Key cache {param.Name} thiêu tham số {string.Join(",", invalidFields)}");
                }
            }

            return cacheKey.ToLower();
        }

        /// <summary>
        /// Map từ name ra cache config
        /// </summary>
        private CacheItemConfig GetItemConfig(CacheItemName name)
        {
            var cacheKey = name.ToString();
            if (!_cacheConfig.Items.ContainsKey(cacheKey))
            {
                throw new KeyNotFoundException($"Không tìm thấy cấu hình cache {cacheKey}");
            }

            var result = _cacheConfig.Items[cacheKey];
            if (string.IsNullOrEmpty(result.Key))
            {
                throw new KeyNotFoundException($"Thiếu cấu hình cache {cacheKey} Format");
            }

            return result;
        }
    }
}
