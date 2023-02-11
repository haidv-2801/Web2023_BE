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
    public class LibraryCardController : BaseEntityController<LibraryCard>
    {
        #region Declare
        ILibraryCardService _libraryCardService;
        ILogger<LibraryCard> _logger;
        #endregion

        #region Constructer
        public LibraryCardController(ILibraryCardService libraryCardService, ILogger<LibraryCard> logger) : base(libraryCardService, logger)
        {
            _libraryCardService = libraryCardService;
            _logger = logger;
        }
        #endregion

        #region Methods
        [EnableCors("AllowCROSPolicy")]
        [AllowAnonymous]
        [HttpGet("/api/LibraryCard/NextCardCode")]
        public async Task<IActionResult> GetNextCardCode()
        {
            return Ok(await _libraryCardService.GetNextCardCode());
        }

        [EnableCors("AllowCROSPolicy")]
        [AllowAnonymous]
        [HttpPost("/api/LibraryCard/AcceptMany")]
        public async Task<IActionResult> AcceptMany(AcceptCardMany acceptMany)
        {
            return Ok(await _libraryCardService.AcceptMany(acceptMany));
        }
        #endregion
    }
}
