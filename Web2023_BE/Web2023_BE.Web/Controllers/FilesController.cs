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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller bài đăng
    /// </summary>
    [ApiController]
    public class FilesController : ControllerBase
    {
        #region Declare
        IHostingEnvironment _hostingEnvironment;
        string FolderName = "Stores";

        private readonly IStorageService _storageService;
        private readonly IContextService _contextService;
        private readonly StorageConfig _storageConfig;

        #endregion

        #region Constructer
        public FilesController(IHostingEnvironment hostingEnvironment,
            IStorageService storageService,
            IContextService contextService,
            StorageConfig storageConfig)
        {
            _hostingEnvironment = hostingEnvironment;
            _storageService = storageService;
            _contextService = contextService;
            _storageConfig = storageConfig;
        }
        #endregion

      
        [EnableCors("AllowCROSPolicy")]
        [Route("/api/file/insert")]
        [HttpPost]
        [DisableFormValueModelBindingAttribute]
        public ActionResult Create([FromBody]IFormFile file)
        {
            try
            {
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, FolderName);
                var filePath = Path.Combine(uploads, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                return Ok(uniqueFileName);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

      


    }
}
