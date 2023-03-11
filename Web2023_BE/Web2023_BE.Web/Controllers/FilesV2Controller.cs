using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore;

namespace Web2023_BE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesV2Controller : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IContextService _contextService;
        private readonly StorageConfig _storageConfig;

        public FilesV2Controller(
            IStorageService storageService,
            IContextService contextService,
            StorageConfig storageConfig
            )
        {
            _storageService = storageService;
            _contextService = contextService;
            _storageConfig = storageConfig;
        }

        /// <summary>
        /// Get file
        /// Trả về file theo content type mặc định
        /// </summary>
        [HttpGet]
        [HttpGet("{type}")]
        [HttpGet("{type}/{file}")]
        [HttpGet("{type}/{file}/{name}")]
        [HttpGet("{type}/{file}/{name}/{dbid}")]
        [EnableCors("AllowCROSPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string file, StorageFileType type, int? dbid, string name)
        {
            string contentType = _storageService.GetContentType(Path.GetExtension(file));
            var result1 = await GetFile(file, type, dbid, name, contentType);
            return result1;
        }

        /// <summary>
        /// force download file
        /// </summary>
        [HttpGet("download")]
        [HttpGet("download/{type}")]
        [HttpGet("download/{type}/{file}")]
        [HttpGet("download/{type}/{file}/{name}")]
        [HttpGet("download/{type}/{dbid}/{file}/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> Download(string file, StorageFileType type, int? dbid, string name)
        {
            var result = await GetFile(file, type, dbid, name, "application/octect-stream");
            return result;
        }

        /// <summary>
        /// Upload file lên server
        /// </summary>
        [HttpPost]
        [RequestSizeLimit(100000000)]
        [EnableCors("AllowCROSPolicy")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var err = ValidateUpload(StorageFileType.Temp, file);
            if (!string.IsNullOrEmpty(err))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
            else
            {
                var context = _contextService.GetContext();
                string fileKey = string.Format(Guid.NewGuid().ToString());
                string fileExtension = Path.GetExtension(file.FileName);
                string fileName = $"{fileKey}{fileExtension}";
                string fileType = _storageService.GetContentType(fileExtension);

                using (var stream = file.OpenReadStream())
                {
                    //chỉ được upload vào temp
                    await _storageService.SaveAsync(StorageFileType.Temp, fileName, stream, context.DatabaseId, contentType: fileType);
                }

                return Ok(fileName);
            }
        }

        /// <summary>
        /// Upload file lên server
        /// </summary>
        /// <param name="type">Upload file gì: Dùng để validate nghiệp vụ, con sẽ dẩy vào temp</param>
        /// <param name="file">Danh sách các file</param>
        /// <returns>Danh sách tên file temp</returns>
        [RequestSizeLimit(100000000)]
        [HttpPost("multi")]
        public async Task<IActionResult> UploadFiles([FromRoute][Required] StorageFileType type, List<IFormFile> file)
        {
            int validCount = 0;
            int errorCount = 0;
            var context = _contextService.GetContext();
            var temps = new List<object>();
            for (var i = 0; i < file.Count; i++)
            {
                var item = file[i];
                var err = ValidateUpload(type, item);
                if (!string.IsNullOrEmpty(err))
                {
                    temps.Add(new { error = err });
                    errorCount++;
                }
                else
                {
                    validCount++;

                    string fileKey = string.Format(Guid.NewGuid().ToString());
                    string fileExtension = Path.GetExtension(item.FileName);
                    string fileName = $"{fileKey}{fileExtension}";
                    string fileType = _storageService.GetContentType(fileExtension);

                    using (var stream = item.OpenReadStream())
                    {
                        //chỉ được upload vào temp
                        await _storageService.SaveAsync(StorageFileType.Temp, fileName, stream, context.DatabaseId, contentType: fileType);
                    }

                    temps.Add(new { name = fileName });
                }
            }
            return Ok(temps);
        }

        /// <summary>
        /// Get image
        /// </summary>
        [HttpGet("image")]
        [HttpGet("image/{type}/{name}")]
        [HttpGet("image/{type}/{name}/{dbid}")]
        [HttpGet("image/{type}/{name}/{width}/{height}/{dbid}")]
        [HttpGet("image/{type}/{name}/{width}/{height}/{dbid}/{quantity}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 1440)]  //cache 1 ngày
        public async Task<IActionResult> GetImageFile(string name, StorageFileType type, int? dbid, int? width, int? height, int? quality)
        {
            //TODO: Thêm logic check loại file được phép get
            var stream = await _storageService.GetAsync(type, name, dbid);
            if (stream != null)
            {
                //resize nếu có truyền kích thước
                if (height > 0 || width > 0)
                {
                    byte[] bytes = stream.ToArray();
                    using (var image = new MagickImage(bytes))
                    {
                        if (height.HasValue && height > 0)
                        {
                            width = image.Width * height / image.Height;
                        }
                        else
                        {
                            height = image.Height * width / image.Width;
                        }

                        image.Resize(width.Value, height.Value);
                        image.Strip();
                        if (quality.HasValue && quality > 50 && quality <= 100)
                        {
                            image.Quality = quality.Value;
                        }
                        else
                        {
                            image.Quality = 80;
                        }

                        using (var ms = new MemoryStream())
                        {
                            image.Write(ms);
                            bytes = ms.ToArray();
                        }
                    }

                    var result1 = new FileContentResult(bytes, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"));
                    return result1;
                }

                var result2 = File(stream, _storageService.GetContentType(Path.GetExtension(name)));
                return result2;
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, $"File not found type {type}, name {name}, dbid {dbid}");
            }
        }

        /// <summary>
        /// Xóa file
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(StorageFileType type, string name)
        {
            await _storageService.DeleteAsync(type, name);
            return Ok();
        }

        /// <summary>
        /// Kiểm tra file temp có tồn tại không
        /// Dùng cho chỗ xử lý async
        /// </summary>
        [HttpGet("check-temp/{name}")]
        public async Task<IActionResult> CheckTempExist(string name)
        {
            var exist = await _storageService.ExistAsync(StorageFileType.Temp, name: name);
            if (exist)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status404NotFound, "File not exist");
        }

        #region Private

        /// <summary>
        /// Kiểm tra việc upload file có hợp lệ không
        /// </summary>
        /// <param name="type">file gì</param>
        /// <param name="file">file</param>
        private string ValidateUpload(StorageFileType type, IFormFile file)
        {
            var data = file.FileName.Split(".");

            //Nếu không có extension -> fail
            if (data.Count() < 2)
            {
                return "File name invalid";
            }

            var ext = data.Last().ToLower();
            if (!_storageConfig.UploadAllowExtensions.Contains(ext))
            {
                return "File extension invalid";
            }

            if (_storageConfig.UploadMaxSizeMB.HasValue && file.Length > _storageConfig.UploadMaxSizeMB.Value * 1024 * 1024)
            {
                return $"File không được lớn hơn {_storageConfig.UploadMaxSizeMB} MB";
            }

            return null;
        }

        /// <summary>
        /// xử lý trả file về client
        /// </summary>
        private async Task<IActionResult> GetFile(string file, StorageFileType type, int? dbid, string name, string contentType)
        {
            //TODO: Thêm logic check loại file được phép get
            var stream = await _storageService.GetAsync(type, file, dbid);

            if (contentType.StartsWith("image/"))
            {
                var result = File(stream, contentType);
                return result;
            }
            else if (contentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                var result = new FileStreamResult(stream, contentType);
                return result;
            }
            else
            {
                string fileName;
                if (!string.IsNullOrEmpty(name))
                {
                    fileName = Path.GetFileNameWithoutExtension(name) + Path.GetExtension(file);
                }
                else
                {
                    fileName = file;
                }

                var result = File(stream, contentType, fileName);
                return result;
            }
        }

        #endregion
    }
}
