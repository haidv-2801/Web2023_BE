using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Cache
{
    /// <summary>
    /// Cấu hình từng item cache
    /// </summary>
    public class CacheItemConfig
    {
        /// <summary>
        /// Định dạng của key lưu trữ
        /// Quyết định scope truy xuất cache
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Thời gian hết hạn của dist cache
        /// Nếu có khai báo thì mới lưu
        /// </summary>
        public int? DistSeconds { get; set; }
        /// <summary>
        /// Thời gian hết hạn của mem cache
        /// Nếu có khai báo thì mới lưu
        /// </summary>
        public int? MemSeconds { get; set; }
        /// <summary>
        /// Tên nhóm, nếu không khai báo thì lấy mặc định item đầu tiên trong config Cache.DistGroup
        /// </summary>
        public string DistGroup { get; set; }
    }
}
