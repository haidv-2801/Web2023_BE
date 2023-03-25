using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
        public ImageManagerController(IBaseService<ImageManager> baseService, IImageManagerService imageManagerService, ILogger<ImageManager> logger) : base(baseService, logger)
        {
            _logger = logger;
            _imageManagerService = imageManagerService;
        }


        [HttpPost]
        [Route("filter-paging-async")]
        [EnableCors("AllowCROSPolicy")]
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
        [Route("Create")]
        [EnableCors("AllowCROSPolicy")]
        public async Task<IActionResult> CreateImage([FromForm] ImageManagerDTO imageManager)
        {
            var serviceResult = new ServiceResult();
            try
            {
                imageManager.Url = String.Format("{0}://{1}{2}/Images/", Request.Scheme, Request.Host, Request.PathBase);
                var entity = await _imageManagerService.CreateImage(imageManager);

                if (entity == null)
                    return NotFound();

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
        public async Task<IActionResult> CreateImage(string id, [FromForm] ImageManagerDTO imageManager)
        {
            var serviceResult = new ServiceResult();
            try
            {
                imageManager.Url = String.Format("{0}://{1}{2}/Images/", Request.Scheme, Request.Host, Request.PathBase);
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
        public async Task<IActionResult> CreateImage(string id)
        {
            var serviceResult = new ServiceResult();
            try
            {

                var entity = await _imageManagerService.DeleteImage(id);

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



        //[EnableCors("AllowCROSPolicy")]
        //[HttpDelete("deleteasync/{id}")]
        //public async Task<IActionResult> DeleteAsync(string id, StorageFileType type, string name)
        //{
        //    var serviceResult = await _imageManagerService.DeleteImage(id, type, name);
        //    if (serviceResult)
        //        return Ok(serviceResult);
        //    else
        //        return NoContent();
        //}
    }
}