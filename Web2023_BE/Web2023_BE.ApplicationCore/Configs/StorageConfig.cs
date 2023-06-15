using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore
{
    /// <summary>
    /// Cấu hình quản lý file
    /// </summary>
    public class StorageConfig
    {
        /// <summary>
        /// danh sách file extension cho phép upload
        /// </summary>
        public string UploadAllowExtensions { get; set; }
        /// <summary>
        /// Dung lượng theo MB của file tối đa cho phép upload
        /// </summary>
        public decimal? UploadMaxSizeMB { get; set; }
        /// <summary>
        /// Dùng cái gì: File, MinIo
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Cấu hình của file system
        /// </summary>
        public StorageFileConfig File { get; set; }

        /// <summary>
        /// Cấu hình của minio
        /// </summary>
        public StorageMinIOConfig MinIO { get; set; }
    }

    public class StorageFileConfig
    {
        public string Real { get; set; }
        public string Temp { get; set; }
    }

    /// <summary>
    /// Cấu hình quản lý file tạm
    /// </summary>
    public class StorageMinIOConfig
    {
        /// <summary>
        /// Cấu hình file thật
        /// </summary>
        public StorageMinIoBucketConfig Real { get; set; }
        /// <summary>
        /// Cấu hình file tạm
        /// </summary>
        public StorageMinIoBucketConfig Temp { get; set; }
    }

    /// <summary>
    /// Cấu hình quản lý file tạm
    /// </summary>
    public class StorageMinIoBucketConfig
    {
        /// <summary>
        /// Endpoint kết nối tới server MISA Storage.
        /// </summary>
        public string ServiceURL { get; set; }

        /// <summary>
        /// Tài khoản (Username) để đăng nhập vào hệ thống MISA Storage
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Thông tin tài khoản (Password) để đăng nhập vào hệ thống MISA Storage
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Tên bucket lưu trữ object được chỉ định coh tài khoản này.
        /// Để thêm path (subfolder) thì cần thêm /pathname vào sau bucket
        /// Công thức: BucketName/path1/path2/..pathn
        /// VD: "smecloud/development/report"
        ///     "smecloud/test/report"
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// tùy chỉnh thư mục lưu trữ file
        /// </summary>
        public string Format { get; set; }
    }
}
