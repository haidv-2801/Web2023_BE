using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.Entities;
using Web2023_BE.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net.NetworkInformation;
using Web2023_BE.ApplicationCore.Helpers;
using Web2023_BE.ApplicationCore.MiddleWare;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller base
    /// </summary>
    [Route("/api/[controller]")]
    [ApiController]
    public class BaseEntityController<TEntity> : ControllerBase
    {
        #region Declare
        IBaseService<TEntity> _baseService;
        private readonly ILogger<TEntity> _logger;
        #endregion

        public BaseEntityController(IBaseService<TEntity> baseService, ILogger<TEntity> logger)
        {
            _baseService = baseService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách thực thể
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// CreatedBy: DVHAI 07/07/2021
        [EnableCors("AllowCROSPolicy")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var entities = await _baseService.GetEntities();

            return Ok(entities);
        }

        [HttpPost]
        [Route("Filter")]
        [EnableCors("AllowCROSPolicy")]
        public virtual async Task<IActionResult> GetFilter(PagingRequest pagingRequest)
        {
            var serviceResult = new ServiceResult();
            try
            {
                _logger.LogInformation($"Filter {typeof(TEntity).Name} info : " + JsonConvert.SerializeObject(pagingRequest));
                var entity = await _baseService.GetEntitiesFilter(pagingRequest);

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

        /// <summary>
        /// Lấy thực thể theo id
        /// </summary>
        /// <param name="id">id của thực thể</param>
        /// <returns>Một thực thể tìm được theo id</returns>
        /// CreatedBy: DVHAI 07/07/2021
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _baseService.GetEntityById(Guid.Parse(id));

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Thêm một thực thể mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Sô bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: DVHAI 07/07/2021
        [EnableCors("AllowCROSPolicy")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            var serviceResult = new ServiceResult();
            try
            {
                _logger.LogInformation($"Thêm bản ghi {typeof(TEntity).Name}: " + JsonConvert.SerializeObject(entity));
                serviceResult = await _baseService.Insert(entity);
                if (serviceResult.Code == Enums.InValid)
                    return BadRequest(serviceResult);
                else if (serviceResult.Code == Enums.Exception || serviceResult.Code == Enums.Fail)
                    return StatusCode(500, serviceResult);

                return StatusCode(201, serviceResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi Insert {typeof(TEntity).Name}: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpPatch("PatchUpdate/{id}")]
        public async Task<IActionResult> Patch(Guid id,
    [FromBody] JsonPatchDocument patchDoc)
        {
            if (patchDoc is null)
            {
                throw new ArgumentNullException(nameof(patchDoc));
            }

            try
            {
                var serviceResult = new ServiceResult();
                if (patchDoc != null)
                {
                    var entity = await _baseService.GetEntityById(id);
                    if (entity == null) return NotFound();
                    else
                    {
                        patchDoc.ApplyTo(entity);
                        serviceResult = await _baseService.Update(id, entity);

                        if (serviceResult.Code == Enums.InValid)
                            return BadRequest(serviceResult);
                        else if (serviceResult.Code == Enums.Exception)
                            return StatusCode(500, serviceResult);

                        return Ok(serviceResult);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return BadRequest();
        }

        /// <summary>
        /// Sửa một thực thể
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <param name="entity">thông tin của bản ghi</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: DVHAI 07/07/2021
        [EnableCors("AllowCROSPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] TEntity entity)
        {
            try
            {
                _logger.LogInformation($"Body put {typeof(TEntity).Name}:" + JsonConvert.SerializeObject(entity));
                //Sử lí kiểu id động ở đây
                var serviceResult = await _baseService.Update(Guid.Parse(id), entity);
                _logger.LogInformation($"ServiceResult Body put {typeof(TEntity).Name}:" + JsonConvert.SerializeObject(serviceResult));

                if (serviceResult.Code == Enums.InValid)
                    return BadRequest(serviceResult);
                else if (serviceResult.Code == Enums.Exception)
                    return StatusCode(500, serviceResult);

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error put {typeof(TEntity).Name}:" + ex.Message);
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: DVHAI 07/07/2021
        [EnableCors("AllowCROSPolicy")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var serviceResult = _baseService.Delete(Guid.Parse(id));
            if (serviceResult.Code == Enums.Success)
                return Ok(serviceResult);
            else
                return NoContent();
        }
    }
}
