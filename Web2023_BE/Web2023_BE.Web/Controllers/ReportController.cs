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
    [Route("/api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        #region Declare
        IBookService _bookService;
        IContactSubmitService _libraryCardService;
        IBookOrderService _bookOrderService;
        #endregion

        #region Constructer
        public ReportController(IBookService bookService, IContactSubmitService libraryCardService, IBookOrderService bookOrderService)
        {
            _bookService = bookService;
            _libraryCardService = libraryCardService;
            _bookOrderService = bookOrderService;
        }
        #endregion

        #region Methods

        [HttpGet]
        [Route("/api/scorecard")]
        [EnableCors("AllowCROSPolicy")]
        public async Task<IActionResult> GetReportScoreCard()
        {
            var totalBookOrdered = _bookOrderService.GetTotalBookOrdered();
            var totalBook = _bookService.GetTotalBook();
            var res = await Task.WhenAll(totalBookOrdered, totalBook);
            return Ok(new { totalBooks = 0, totalBookOrdereds = res[0], totalLibraryCards = res[1] });
        }


        [HttpGet]
        [Route("/api/top-book-borrowed")]
        [EnableCors("AllowCROSPolicy")]
        public async Task<IActionResult> GetTopBookBorrowed()
        {
            return Ok(await _bookOrderService.TopBookBorrowed());
        }
        #endregion
    }
}
