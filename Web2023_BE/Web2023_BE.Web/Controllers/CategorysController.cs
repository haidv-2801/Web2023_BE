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

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller bài đăng
    /// </summary>
    [ApiController]
    public class CategorysController : BaseEntityController<Category>
    {
        #region Declare
        IBaseService<Category> _categoryService;
        #endregion

        #region Constructer
        public CategorysController(IBaseService<Category> menuService, ILogger<Category> logger) : base(menuService, logger)
        {
            _categoryService = menuService;
        }
        #endregion

        #region Methods
        #endregion
    }
}
