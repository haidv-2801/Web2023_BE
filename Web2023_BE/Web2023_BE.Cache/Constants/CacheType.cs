using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Cache.Constants
{
    /// <summary>
    /// Loại cache
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// Dùng chung cho toàn ứng dụng
        /// </summary>
        Global,
        /// <summary>
        /// Theo dữ liệu
        /// </summary>
        Database,
        /// <summary>
        /// Theo người dùng
        /// </summary>
        User,
        /// <summary>
        /// Theo người dùng trong dữ liệu
        /// </summary>
        UserDatabase,
        /// <summary>
        /// Theo phiên đăng nhập
        /// </summary>
        Session
    }
}
