using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Data;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.Web.Controllers;
using Microsoft.AspNetCore.Cors;
using Web2023_BE.Entities;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller bài đăng
    /// </summary>
    [ApiController]
    public class SafeAddressController : BaseEntityController<SafeAddress>
    {
        #region Declare
        ISafeAddressService _safeAddressService;
        ILogger<SafeAddress> _logger;
        #endregion

        #region Constructer
        public SafeAddressController(ISafeAddressService safeAddressService, ILogger<SafeAddress> logger) : base(safeAddressService, logger)
        {
            _safeAddressService = safeAddressService;
            _logger = logger;
        }
        #endregion

        #region Methods
        [HttpGet("CheckAllowedAccess")]
        [EnableCors("AllowCROSPolicy")]
        public async Task<IActionResult> CheckIsInNetworkAllowed()
        {
            var entities = await _safeAddressService.CheckIsInNetworkAllowed();
            return Ok(entities);
        }

        [HttpPost]
        [Route("Filter")]
        [EnableCors("AllowCROSPolicy")]
        public override async Task<IActionResult> GetFilter(PagingRequest pagingRequest)
        {
            var serviceResult = new ServiceResult();
            try
            {
                _logger.LogInformation($"Filter {typeof(SafeAddress).Name} info : " + JsonConvert.SerializeObject(pagingRequest));
                var entity = await _safeAddressService.GetEntitiesFilter(pagingRequest, "safe_address");

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
        #endregion
    }
}
