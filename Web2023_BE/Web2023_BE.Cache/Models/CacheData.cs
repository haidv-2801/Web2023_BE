using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Cache
{
    /// <summary>
    /// Dữ liệu lưu vào cache
    /// Wrap để phân biệt không có trong cache và cache lưu giá trị default(T)
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của cache data</typeparam>
    public class CacheData<T>
    {
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public T Value { get; set; }
    }
}
