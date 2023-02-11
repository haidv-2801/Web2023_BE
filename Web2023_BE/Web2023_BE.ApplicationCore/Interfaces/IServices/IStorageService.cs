using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore
{
    /// <summary>
    /// Xử lý thao tác với các file
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Lấy file theo loại và tên file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">tên file</param>
        /// <param name="databaseId">dữ liệu nào</param>
        /// <returns>Stream</returns>
        Task<MemoryStream> GetAsync(StorageFileType type, string name = null, int? databaseId = null);
        /// <summary>
        /// Lấy file theo loại và tên file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">tên file</param>
        /// <param name="databaseId">dữ liệu nào</param>
        /// <returns>String content</returns>
        Task<string> GetStringAsync(StorageFileType type, string name = null, int? databaseId = null);

        /// <summary>
        /// Lưu file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">Tên file</param>
        /// <param name="content">Nội dung file</param>
        /// <param name="databaseId">Dữ liệu nào, nếu có truyền sẽ lưu vào thư mực đặc thù của đơn vị</param>
        Task SaveAsync(StorageFileType type, string name, Stream content, int? databaseId = null, string contentType = "text/plain");

        /// <summary>
        /// Lưu file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">Tên file</param>
        /// <param name="content">Nội dung file</param>
        /// <param name="databaseId">Dữ liệu nào, nếu có truyền sẽ lưu vào thư mực đặc thù của đơn vị</param>
        Task SaveAsync(StorageFileType type, string name, string content, int? databaseId = null);

        /// <summary>
        /// Xóa file
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">Tên file</param>
        /// <param name="databaseId">dữ liệu nào</param>
        Task DeleteAsync(StorageFileType type, string name, int? databaseId = null);

        /// <summary>
        /// Copy file từ temp sang thư mục thật
        /// Sau khi copy sẽ xóa file temp đi
        /// </summary>
        /// <param name="tempName">Tên file temp</param>
        /// <param name="toType">Loại file đích</param>
        /// <param name="toName">Tên file đích, nếu không</param>
        /// <param name="databaseId">dữ liệu nào</param>
        Task CopyTempToRealAsync(string tempName, StorageFileType toType, string toName, int? databaseId);

        /// <summary>
        /// Lấy danh sách tên file trong thư mục
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="subFolder">Thư mục con</param>
        /// <returns>Danh sách tên file</returns>
        Task<List<string>> GetFileNamesAsync(StorageFileType type, string subFolder = null);

        /// <summary>
        /// Kiểm tra file có tồn tại không
        /// </summary>
        /// <param name="type">Loại file</param>
        /// <param name="name">tên file</param>
        Task<bool> ExistAsync(StorageFileType type, string name = null, int? databaseId = null);

        /// <summary>
        /// Lấy ContentType dựa vào đuôi file
        /// </summary>
        /// <param name="extension">Đuôi file dạng: .xls, .doc, .pdf...</param>
        /// <returns>ContentType dạng application/excel</returns>
        string GetContentType(string extension);
    }
}
