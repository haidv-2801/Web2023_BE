using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Interfaces.IServices;
using Web2023_BE.Entities;

namespace Web2023_BE.Web.Controllers
{
    public class ImageManagerController : BaseEntityController<ImageManager>
    {
        ILogger<ImageManager> _logger;
        private readonly IImageManagerService _imageManagerService;
        private readonly StorageConfig _storageConfig;
        private readonly string baseUrl = "images/";

        public ImageManagerController(IBaseService<ImageManager> baseService, IImageManagerService imageManagerService, StorageConfig storageConfig, ILogger<ImageManager> logger) : base(baseService, logger)
        {
            _logger = logger;
            _imageManagerService = imageManagerService;
            _storageConfig = storageConfig;
        }


        [HttpPost]
        [Route("filter-paging-async")]
        [EnableCors("AllowCROSPolicy")]
        [Authorize]
        public async Task<IActionResult> GetListImagePagingAsync(PagingRequest pagingRequest)
        {
            var serviceResult = new ServiceResult();
            try
            {
                _logger.LogInformation($"Filter {typeof(Folder).Name} info : " + JsonConvert.SerializeObject(pagingRequest));
                var entity = await _imageManagerService.GetListImagePagingAsync(pagingRequest);

                if (entity == null)
                    return NotFound();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi GetFilter: " + ex.Message);
                serviceResult.Data = null;
                serviceResult.Messasge = ex.Message;
                serviceResult.Code = Enums.Fail;
            }

            if (serviceResult.Code == Enums.Fail) { return BadRequest(serviceResult); }

            return Ok(serviceResult);
        }


        [HttpPost]
        [Route("create")]
        [RequestSizeLimit(100000000)]
        [EnableCors("AllowCROSPolicy")]
        [Authorize]
        public async Task<IActionResult> CreateImage([FromForm] IFormFile image)
        {
            var serviceResult = new ServiceResult();
            try
            {
                var imageManager = new ImageManagerDTO()
                {
                    Url = baseUrl,
                    ImageFile = image
                };

                var err = ValidateUpload(image);

                if (!string.IsNullOrEmpty(err))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, err);
                }

                var entity = await _imageManagerService.CreateImage(imageManager);

                if (entity == null)
                    return NotFound();

                if (!entity.IsSuccess)
                    return BadRequest(entity);

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi create: " + ex.Message);
                serviceResult.Data = null;
                serviceResult.Messasge = ex.Message;
                serviceResult.Code = Enums.Fail;
            }

            if (serviceResult.Code == Enums.Fail) { return BadRequest(serviceResult); }

            return Ok(serviceResult);
        }


        [HttpPut]
        [Route("update/{id}")]
        [EnableCors("AllowCROSPolicy")]
        [Authorize]
        public async Task<IActionResult> CreateImage(string id, [FromForm] ImageManagerDTO imageManager)
        {
            var serviceResult = new ServiceResult();
            try
            {
                //imageManager.Url = String.Format("{0}://{1}{2}/Images/", Request.Scheme, Request.Host, Request.PathBase);
                var entity = await _imageManagerService.UpdateImage(id, imageManager);

                if (entity == null)
                    return NotFound();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi uodate: " + ex.Message);
                serviceResult.Data = null;
                serviceResult.Messasge = ex.Message;
                serviceResult.Code = Enums.Fail;
            }

            if (serviceResult.Code == Enums.Fail) { return BadRequest(serviceResult); }

            return Ok(serviceResult);
        }

        [HttpPut]
        [Route("delete/{id}")]
        [EnableCors("AllowCROSPolicy")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            var serviceResult = new ServiceResult();
            try
            {

                var entity = await _imageManagerService.DeleteImage(Guid.Parse(id));

                if (entity == null)
                    return NotFound();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi delete: " + ex.Message);
                serviceResult.Data = null;
                serviceResult.Messasge = ex.Message;
                serviceResult.Code = Enums.Fail;
            }

            if (serviceResult.Code == Enums.Fail) { return BadRequest(serviceResult); }

            return Ok(serviceResult);
        }



        /// <summary>
        /// Kiểm tra việc upload file có hợp lệ không
        /// </summary>
        /// <param name="file">file</param>
        private string ValidateUpload(IFormFile file)
        {
            var data = file.FileName.Split(".");

            //Nếu không có extension -> fail
            if (data.Count() < 2)
            {
                return "Tên file không hợp lệ";
            }

            var ext = data.Last().ToLower();
            if (!_storageConfig.UploadAllowExtensions.Contains(ext))
            {
                return "Loại file không hợp lệ";
            }

            if (_storageConfig.UploadMaxSizeMB.HasValue && file.Length > _storageConfig.UploadMaxSizeMB.Value * 1024 * 1024)
            {
                return $"File không được lớn hơn {_storageConfig.UploadMaxSizeMB} MB";
            }

            return null;
        }
    }
}