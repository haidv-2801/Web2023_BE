
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.Cache;
using Web2023_BE.Cache.Constants;

namespace Web2023_BE.ApplicationCore
{
    /// <summary>
    /// Class base cho các provider xử lý file
    /// </summary>
    public class BaseStorageService
    {
        /// <summary>
        /// Cấu hình quản lý file
        /// </summary>
        IConfiguration _config;
        protected readonly StorageConfig _storageConfig;
        protected readonly ICacheService _cacheService;
        private string _defaultFolder;

        /// <summary>
        /// Khởi tạo
        /// </summary>
        public BaseStorageService(
            IConfiguration config,
            StorageConfig storageConfig,
            ICacheService cacheService
            )
        {
            _config = config;
            _storageConfig = storageConfig;
            _cacheService = cacheService;
            _defaultFolder = this.GetDefaultFolder();
        }

        /// <summary>
        /// Lấy đường dẫn cấu trúc thư mục chứa file
        /// Sẽ lower để tránh lỗi trên linux
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">tên file</param>
        /// <param name="databaseId">dữ liệu nào</param>
        protected virtual string GetPath(StorageFileType type, string name, int? databaseId)
        {
            //xử lý chống truy cập trái phép    
            string fileName = string.IsNullOrEmpty(name) ? "" : Path.GetFileName(name);

            string path;

            /* Nếu sử dụng trên AWS S3 thì sẽ không ghép thêm path Environment vào do trên đó đã lưu trữ rồi mà không muốn convert vào thư mục theo Env nữa
             * Các môi trường khác MinIo hoặc File (trên development) thì sẽ lưu riêng theo từng môi trường
             */
            //var envName = GetEnvironmentName();
            switch (type)
            {
                case StorageFileType.Temp:
                    //một số file hệ thống sẽ xử lý không có tenant
                    path = this.GetTempPath(fileName);
                    break;

                case StorageFileType.Thumbnail:
                    path = this.GetTempPath(fileName);
                    break;
                case StorageFileType.SqlUpgradeTenant:
                case StorageFileType.SqlUpgradeManage:
                case StorageFileType.SqlUpgradeSystem:
                case StorageFileType.SqlUpgradeQueue:
                    //script nâng cấp db
                    if (!string.IsNullOrEmpty(name) && name.Contains("../"))
                    {
                        throw new Exception($"Parameter name {name} invalid");
                    }
                    path = Path.Combine(this.GetRootPath(), $"SqlUpgrade/{type.ToString().Replace("SqlUpgrade", "")}/{name}");
                    break;
                default:
                    /**
                     * Mặc định thư mục lưu file là {typeName}/{fileName}
                     * Với file theo dữ liệu thì {databaseId}/{typeName}/{fileName}
                     */
                    path = Path.Combine(this.GetStorePath(databaseId), type.ToString(), fileName);
                    break;
            }

            return path.Replace(@"\", "/");
        }

        /// <summary>
        /// Lấy đường dẫn file tạm
        /// </summary>
        /// <param name="fileName">tên file</param>
        protected virtual string GetTempPath(string fileName)
        {
            return Path.Combine(this.GetRootPath(), "temp", fileName);
        }

        /// <summary>
        /// Lấy thư mục gốc lưu file
        /// </summary>
        /// <param name="databaseId">Dữ liệu công ty nào, nếu không truyền là dữ liệu mặc định mang đi</param>
        protected string GetStorePath(int? databaseId)
        {
            string subPath;
            if (databaseId.HasValue)
            {
                subPath = Path.Combine("Tenant", databaseId.ToString());
            }
            else
            {
                subPath = _defaultFolder;
            }
            //var envName = GetEnvironmentName();
            return Path.Combine(this.GetRootPath(), subPath);
        }

        /// <summary>
        /// Lấy thư mục mặc định
        /// </summary>
        protected virtual string GetDefaultFolder()
        {
            return "Default";
        }

        /// <summary>
        /// Lấy thư mục gốc
        /// </summary>
        protected virtual string GetRootPath()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hàm đọc string sẽ kiểm tra nếu là đọc file mang đi sẽ gọi qua cache mem trước, không có thì mới gọi service đọc file
        /// </summary>
        public async Task<string> GetStringAsync(StorageFileType type, string name = null, int? databaseId = null)
        {
            string content;
            if (databaseId == null)
            {
                var cacheParam = new CacheParam()
                {
                    Name = CacheItemName.FileSystemStringContent,
                    Custom = $"{type}{name}"
                };
                content = _cacheService.Get<string>(cacheParam);
                if (string.IsNullOrEmpty(content))
                {
                    content = await this.GetFileStringAsync(type, name, databaseId);
                    if (!string.IsNullOrEmpty(content))
                    {
                        _cacheService.Set(cacheParam, content);
                    }
                }
            }
            else
            {
                content = await this.GetFileStringAsync(type, name, databaseId);
            }

            return content;
        }

        /// <summary>
        /// Đọc nội dung file mang đi
        /// </summary>
        protected virtual async Task<string> GetFileStringAsync(StorageFileType type, string name = null, int? databaseId = null)
        {
            string content = "";
            using (var stream = await this.GetAsync(type, name, databaseId))
            {
                StreamReader reader = new StreamReader(stream);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        /// <summary>
        /// Đọc nội dung file
        /// </summary>
        public virtual async Task<MemoryStream> GetAsync(StorageFileType type, string name = null, int? databaseId = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy ContentType dựa vào đuôi file
        /// </summary>
        /// <param name="extension">Đuôi file dạng: .xls, .doc, .pdf...</param>
        /// <returns>ContentType dạng application/excel</returns>
        public string GetContentType(string extension)
        {
            string type;
            switch (extension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                    type = "application/vnd.ms-excel";
                    break;
                case ".doc":
                case ".docx":
                    type = "application/msword";
                    break;
                case ".pdf":
                    type = "application/pdf";
                    break;
                case ".zip":
                    type = "application/x-zip-compressed";
                    break;
                case ".png":
                    type = "image/png";
                    break;
                case ".gif":
                    type = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    type = "image/jpg";
                    break;
                case ".tif":
                    type = "image/tif";
                    break;
                case ".css":
                    type = "text/css";
                    break;
                case ".js":
                    type = "text/js";
                    break;
                case ".svg":
                    type = "image/svg+xml";
                    break;
                default:
                    type = "text/plain";
                    break;
            }

            return type;
        }
    }
}
