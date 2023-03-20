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
    /// Controller book order
    /// </summary>
    [ApiController]
    public class BookOrderController : BaseEntityController<BookOrder>
    {
        #region Declare
        IBookOrderService _bookOrderService;
        IBaseService<BookOrderViewDTO> _bookOrderViewService;
        ILogger<BookOrder> _logger;
        #endregion

        #region Constructer
        public BookOrderController(IBookOrderService bookOrderService, IBaseService<BookOrderViewDTO> bookOrderViewService, ILogger<BookOrder> logger) : base(bookOrderService, logger)
        {
            _bookOrderService = bookOrderService;
            _bookOrderViewService = bookOrderViewService;
            _logger = logger;
        }
        #endregion

        #region Methods
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("Insert")]
        public async Task<IActionResult> Post([FromBody] BookOrder bookOrder)
        {
            var serviceResult = new ServiceResult();
            try
            {
                _logger.LogInformation($"Thêm bản ghi {typeof(BookOrder).Name}: " + JsonConvert.SerializeObject(bookOrder));
                serviceResult = await _bookOrderService.InsertBookOrder(bookOrder);
                if (serviceResult.Code == Enums.InValid || serviceResult.Code == Enums.InValid)
                    return BadRequest(serviceResult);
                else if (serviceResult.Code == Enums.Exception || serviceResult.Code == Enums.Fail)
                    return StatusCode(500, serviceResult);

                return StatusCode(201, serviceResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi Insert {typeof(BookOrder).Name}: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpGet("NextBookOrderCode")]
        public async Task<IActionResult> GetNextBookOrderCode()
        {
            try
            {
                return Ok(_bookOrderService.GetNextBookOrderCode());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [EnableCors("AllowCROSPolicy")]
        [HttpPost("BookOrderFilterV2")]
        public async Task<IActionResult> GetBookOrderFilterV2(PagingRequest pagingRequest)
        {
            try
            {
                return Ok(await _bookOrderViewService.GetEntitiesFilter(pagingRequest, "view_bookorderview"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
