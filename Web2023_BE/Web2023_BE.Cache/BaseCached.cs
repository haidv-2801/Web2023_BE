using Web2023_BE.Cache.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Cache
{
    public abstract class BaseCached : ICached
    {
        #region "Public Method"        
        public T Get<T>(string key, bool removeAfterGet = false)
        {
            var cacheKey = this.GetCachekey(key);
            var result = this.GetCache<T>(cacheKey);

            //Nếu có cờ xóa sau khi đọc thì gọi xóa luôn
            if (removeAfterGet && !object.Equals(default(T), result))
            {
                this.RemoveCache(cacheKey);
            }
            return result;
        }

        public void Remove(string key)
        {
            var cacheKey = this.GetCachekey(key);
            this.RemoveCache(cacheKey);
        }

        public void Set<T>(string key, T data, TimeSpan? expired = null)
        {
            var cacheKey = this.GetCachekey(key);
            var cacheExpired = this.GetExpired(CacheType.Global, expired);
            this.SetCache(cacheKey, data, cacheExpired);
        }
        #endregion

        /// <summary>
        /// Xử lý key cache
        /// </summary>
        protected virtual string GetCachekey(string key)
        {
            return key;
        }

        /// <summary>
        /// Xử lý thời gian expired của cache
        /// Gom lại để nếu tình huống không gán thì sẽ đặt mặc định
        /// Không để cache item tồn tại mãi dc
        /// </summary>
        protected TimeSpan GetExpired(CacheType type, TimeSpan? expired)
        {
            if (expired != null)
            {
                return expired.Value;
            }

            switch (type)
            {
                case CacheType.Global:
                    return TimeSpan.FromSeconds(4 * 60 * 60);
                case CacheType.Database:
                    return TimeSpan.FromSeconds(2 * 60 * 60);
                case CacheType.User:
                    return TimeSpan.FromSeconds(1 * 60 * 60);
                case CacheType.UserDatabase:
                    return TimeSpan.FromSeconds(1 * 60 * 60);
                case CacheType.Session:
                    return TimeSpan.FromSeconds(30 * 60);
            }

            return TimeSpan.FromSeconds(60 * 60);
        }

        /// <summary>
        /// Gán cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="data">Giá trị</param>
        /// <param name="expired">Thời gian tồn tại</param>
        protected virtual void SetCache<T>(string key, T data, TimeSpan expired)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Đọc cache
        /// </summary>
        /// <typeparam name="T">Dữ liệu gì</typeparam>
        /// <param name="key">Key</param>
        protected virtual T GetCache<T>(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Đọc cache
        /// </summary>
        /// <param name="key">Key</param>
        protected virtual void RemoveCache(string key)
        {
            throw new NotImplementedException();
        }
    }
}
