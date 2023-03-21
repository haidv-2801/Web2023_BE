
using Web2023_BE.Domain.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.Cache;
using Microsoft.Extensions.Configuration;

namespace Web2023_BE.ApplicationCore.FileSystem
{
    public class FileStorageService : BaseStorageService, IStorageService
    {
        /// <summary>
        /// Cấu hình custom từ api trỏ đến thư mục lưu file
        /// </summary>
        private const string FILE_CUSTOM_PATH = "FILE_CUSTOM_PATH";

        public FileStorageService(
            IConfiguration  config,
            StorageConfig storageConfig,
            ICacheService cacheService) : base(config, storageConfig, cacheService)
        {
        }

        protected override string GetRootPath()
        {
            var customPath = Environment.GetEnvironmentVariable(FILE_CUSTOM_PATH) ?? "";
            string basePath = AppContext.BaseDirectory;
            var rootPath = Path.Combine(basePath, customPath, "Stores");
            return rootPath;
        }

        public async Task CopyTempToRealAsync(string tempName, StorageFileType toType, string toName, int? databaseId)
        {
            var tempPath = this.GetPath(StorageFileType.Temp, tempName, null);
            if (System.IO.File.Exists(tempPath))
            {
                var toPath = this.GetPath(toType, toName, databaseId);

                //Tạo thư mục nếu chưa có
                this.CreateDirectoryIfNotExist(toPath);

                //copy file
                System.IO.File.Copy(tempPath, toPath);

                //Xóa file temp đi
                System.IO.File.Delete(tempPath);
            }
        }

        public async Task DeleteAsync(StorageFileType type, string name, int? databaseId = null)
        {
            if (databaseId.HasValue || type == StorageFileType.Temp)
            {
                var path = this.GetPath(type, name, databaseId);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        public override async Task<MemoryStream> GetAsync(StorageFileType type, string name = null, int? databaseId = null)
         {
            string path = this.GetPath(type, name, databaseId);

            if (System.IO.File.Exists(path))
            {
                return new MemoryStream(System.IO.File.ReadAllBytes(path));
            }

            return null;
        }

        public async Task<List<string>> GetFileNamesAsync(StorageFileType type, string subFolder = null)
        {
            var result = new List<string>();
            var path = this.GetPath(type, subFolder, null);

            if (Directory.Exists(path))
            {
                result = Directory.GetFiles(path).Select(n => Path.GetFileName(n)).ToList();
            }

            return result;
        }

        public async Task SaveAsync(StorageFileType type, string name, Stream content, int? databaseId = null, string contentType = "text/plain")
        {
            var path = this.GetPath(type, name, databaseId);

            //Khởi tạo thư mục nếu chưa tồn tại
            this.CreateDirectoryIfNotExist(path);


            //lưu file
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                content.Seek(0, SeekOrigin.Begin);
                await content.CopyToAsync(file);
            }

            var pathThumbnail = this.GetPath(StorageFileType.Thumbnail, "thumbnail", databaseId);


            this.CreateDirectoryIfNotExist(pathThumbnail);

            using (var file = new FileStream(pathThumbnail, FileMode.Create, FileAccess.Write))
            {
                content.Seek(0, SeekOrigin.Begin);
                await content.CopyToAsync(file);
            }




        }

        public async Task SaveAsync(StorageFileType type, string name, string content, int? databaseId = null)
        {
            var path = this.GetPath(type, name, databaseId);

            //Khởi tạo thư mục nếu chưa tồn tại
            this.CreateDirectoryIfNotExist(path);

            //lưu file
            System.IO.File.WriteAllText(path, content);
        }

        /// <summary>
        /// Khởi tạo thư mục để lưu file nếu không tồn tại
        /// </summary>
        /// <param name="filePath">đường dẫn file lưu trữ</param>
        private void CreateDirectoryIfNotExist(string filePath)
        {
            var folder = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public async Task<List<string>> GetDirectorys(StorageFileType type, string subFolder = null)
        {
            var result = new List<string>();
            var path = this.GetPath(type, subFolder, null);

            if (Directory.Exists(path))
            {
                var folders = Directory.GetDirectories(path);
                foreach (var item in folders)
                {
                    result.Add(item.Split('/').Last());
                }
            }

            return result;
        }

        public async Task<bool> ExistAsync(StorageFileType type, string name = null, int? databaseId = null)
        {
            string path = this.GetPath(type, name, databaseId);

            if (System.IO.File.Exists(path))
            {
                return true;
            }

            return false;
        }
    }
}
