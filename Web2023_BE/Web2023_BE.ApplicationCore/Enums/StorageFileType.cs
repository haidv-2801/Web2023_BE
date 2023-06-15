using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore
{
    /// <summary>
    /// Các loại file lưu trữ
    /// </summary>
    public enum StorageFileType
    {
        /// <summary>
        /// File tạm
        /// </summary>
        Temp = 0,
        /// <summary>
        /// Các file dữ liệu mang đi
        /// </summary>
        SystemData = 1,
        /// <summary>
        /// Script sql của đăng ký dữ liệu
        /// </summary>
        Register = 2,
        /// <summary>
        /// File đính kèm
        /// </summary>
        Attachment = 3,
        /// <summary>
        /// Script nâng cấp dữ liệu khách hàng
        /// Trong này sẽ chia các thư mục nhỏ theo từng bản build
        /// 1.0.0.1, 1.0.0.2....
        /// </summary>
        SqlUpgradeTenant = 4,
        /// <summary>
        /// Script nâng cấp dữ liệu system
        /// Trong này sẽ chia các thư mục nhỏ theo từng bản build
        /// 1.0.0.1, 1.0.0.2....
        /// </summary>
        SqlUpgradeSystem = 5,
        /// <summary>
        /// Script nâng cấp dữ liệu manage
        /// Trong này sẽ chia các thư mục nhỏ theo từng bản build
        /// 1.0.0.1, 1.0.0.2....
        /// </summary>
        SqlUpgradeManage = 6,
        /// <summary>
        /// file script nâng cấp db queue
        /// </summary>
        SqlUpgradeQueue = 7,

        Thumbnail = 8,
        
    }
}
