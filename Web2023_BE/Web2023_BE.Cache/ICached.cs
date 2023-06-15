using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Cache
{
    public interface ICached
    {
        /// <summary>
        /// Hàm này dùng để lưu cached
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng muốn cached</typeparam>
        /// <param name="key">Key để lưu cached</param>
        /// <param name="data">Dữ liệu lưu cached</param>
        /// <param name="expired">Thời gian hết hạn cached</param>
        void Set<T>(string key, T data, TimeSpan? expired = null);

        /// <summary>
        /// Lấy giá trị trong cache
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng muốn lấy trong cached</typeparam>
        /// <param name="key">Key lưu cached</param>
        /// <returns></returns>
        T Get<T>(string key, bool removeAfterGet = false);

        /// <summary>
        /// Xóa giá trị trong cached
        /// </summary>
        /// <param name="key">Key lưu cached</param>
        void Remove(string key);
    }
}
