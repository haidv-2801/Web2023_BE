using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Interfaces;
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
                serviceResult.TOECode = TOECode.Fail;
            }

            if (serviceResult.TOECode == TOECode.Fail) { return BadRequest(serviceResult); }

            return Ok(serviceResult);
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpDelete("deleteasync/{id}")]
        public  async Task<IActionResult> DeleteAsync(string id, StorageFileType type, string name)
        {
            var serviceResult = await _imageManagerService.DeleteImage(id, type, name);
            if (serviceResult)
                return Ok(serviceResult);
            else
                return NoContent();
        }
    }
}
