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
using Web2023_BE.Web.Controllers;
using Microsoft.AspNetCore.Cors;
using Web2023_BE.Entities;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller bài đăng
    /// </summary>
    [ApiController]
    public class PostsController : BaseEntityController<Post>
    {
        #region Declare
        IPostService _postService;
        ILogger<Post> _logger;
        #endregion

        #region Constructer
        public PostsController(IPostService postService, ILogger<Post> logger) : base(postService, logger)
        {
            _postService = postService;
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
        [Route("/api/Posts/PostsFilterPaging")]
        [HttpPost]
        public ActionResult GetPostsFilterPaging([FromQuery]string filterValue, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            return Ok(_postService.GetPostsFilterPaging(filterValue, pageSize, pageNumber));           
        }

        [EnableCors("AllowCROSPolicy")]
        [Route("/api/PostsByMenuID/{id}")]
        [HttpGet]
        public ActionResult GetPostsByMenuID([Required] string id)
        {
            return Ok(_postService.GetPostsByMenuID(Guid.Parse(id)));
        }
        #endregion
    }
}
