using Web2023_BE.Cache.Constants;

using System;
using System.Collections.Generic;
using System.Text;


namespace Web2023_BE.Cache
{
    /// <summary>
    /// Service thao tác với cache
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gán cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu lưu trữ</typeparam>
        /// <param name="param">Tham số</param>
        /// <param name="data">Dữ liệu</param>
        /// <returns>Trả về expired time (seconds)</returns>
        int Set<T>(CacheParam param, T data);
        /// <summary>
        /// Đọc cache
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu đọc ra</typeparam>
        /// <param name="param">Tham số</param>
        /// <param name="removeAfterGet">Xóa luôn sau khi đọc ra</param>
        T Get<T>(CacheParam param, bool removeAfterGet = false);
        /// <summary>
        /// Xóa cache
        /// </summary>
        /// <param name="param">Tham số</param>
        void Remove(CacheParam param);
    }
}
