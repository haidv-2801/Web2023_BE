using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Cache.Constants
{
    /// <summary>
    /// Tên cache.items khai báo trong file Cache.json
    /// Làm như này để tiện cho quản trị sau này
    /// Khi thêm item ghi đủ mô tả cho builder sau này tiện quản lý
    /// </summary>
    public enum CacheItemName
    {
        /// <summary>
        /// Thông tin phiên đăng nhập. Được khởi tạo tại AuthApi
        /// </summary>
        ContextData,
        /// <summary>
        /// Chuỗi kết nối vào database
        /// </summary>
        DatabaseConnection,
        /// <summary>
        /// Nội dung trang index trả về client
        /// Dùng cho app api để xử lý hosting cdn
        /// </summary>
        CdnIndex,
        /// <summary>
        /// Thông tin router để điều hướng theo phiên bản của request
        /// </summary>
        VersionMaping,
        /// <summary>
        /// Nội dung file mang đi
        /// </summary>
        FileSystemStringContent,
        /// <summary>
        /// Tham số xuất khẩu
        /// </summary>
        ExportParam,
    }
}
