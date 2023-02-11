
using System;
using System.Collections.Generic;
using System.Text;
using Web2023_BE.Cache.Constants;

namespace Web2023_BE.Cache
{
    /// <summary>
    /// Tham số khi thao tác với cache
    /// Dùng dữ liệu này để tạo ra cache key
    /// </summary>
    public class CacheParam
    {
        /// <summary>
        /// Tên cache
        /// </summary>
        public CacheItemName Name { get; set; }
        /// <summary>
        /// Tài khoản nào
        /// </summary>
        public object UserId { get; set; }
        /// <summary>
        /// Phiên đăng nhập nào
        /// </summary>
        public object SessionId { get; set; }
        /// <summary>
        /// Thông tin Guid, nếu không truyền thì khi sử dụng sẽ new
        /// </summary>
        public Guid? Guid { get; set; }
        /// <summary>
        /// Giá trị custom cho tùy chỉnh
        /// </summary>
        public string Custom { get; set; }
    }
}
