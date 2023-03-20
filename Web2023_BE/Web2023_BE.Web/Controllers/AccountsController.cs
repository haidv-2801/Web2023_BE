using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Data;
using Web2023_BE.ApplicationCore;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Entities;
using Microsoft.AspNetCore.Cors;
using Web2023_BE.Entities;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller bài đăng
    /// </summary>
    [ApiController]
    public class AccountsController : BaseEntityController<Account>
    {
        #region Declare
        IAccountService _accountService;
        ILogger<Account> _logger;
        #endregion

        #region Constructer
        public AccountsController(IAccountService accountService, ILogger<Account> logger) : base(accountService, logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách bài đăng phân trang, tìm kiếm
        /// </summary>
        /// <param name="filterValue">Giá trị tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">Số trang</param>
        /// <returns>Danh sách bài đăng</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        [EnableCors("AllowCROSPolicy")]
        [Route("/api/Accounts/AccountsFilterPaging")]
        [HttpPost]
        public ActionResult GetAccountsFilterPaging([FromQuery] string filterValue, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(_accountService.GetAccountsFilterPaging(filterValue, pageSize, pageNumber));
        }

        /// <summary>
        /// Test Authorization
        /// </summary>
        [EnableCors("AllowCROSPolicy")]
        [Route("test")]
        [HttpGet]
        public IActionResult TestAuth()
        {
            return Ok(new string[] { "value1", "value2", "value3", "value4", "value5" });
        }

        /// <summary>
        /// Login
        /// </summary>
        [EnableCors("AllowCROSPolicy")]
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDTO accountInfo)
        {
            try
            {
                var loggedIn = await _accountService.Login(accountInfo);
                if (loggedIn == null) return Unauthorized();
                return Ok(loggedIn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary>
        /// Login
        /// </summary>
        [EnableCors("AllowCROSPolicy")]
        [AllowAnonymous]
        [HttpPut("/api/Accounts/ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword([FromRoute][Required] string id, [FromBody][Required] AccountPasswordChangeDTO entity)
        {
            var res = new ServiceResult();
            try
            {
                res = await _accountService.ChangePassword(Guid.Parse(id), entity);
                if (res.Code == Enums.InValid || res.Code == Enums.Fail)
                    return BadRequest(res);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        #endregion
    }
}
