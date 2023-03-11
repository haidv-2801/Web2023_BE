using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.MiddleWare;
using Web2023_BE.Entities;

namespace Web2023_BE.Web.Controllers
{
    public class FoldersController : BaseEntityController<Folder>
    {
        private readonly IFolderService _folderService;
        ILogger<Folder> _logger;
        public FoldersController(IFolderService folderService, IBaseService<Folder> baseService, ILogger<Folder> logger) : base(baseService, logger)
        {
            _folderService = folderService;
            _logger = logger;
        }

        #region Methods
        [HttpPost]
        [Route("private/filter")]
        [EnableCors("AllowCROSPolicy")]

        public async Task<IActionResult> GetFilterPrivate(MoveResourceDTO moveResourceDTO)
        {
            var serviceResult = new ServiceResult();
            try
            {
                _logger.LogInformation($"Filter {typeof(Folder).Name} info : " + JsonConvert.SerializeObject(moveResourceDTO));
                var entity = await _folderService.MoveFolderAsync(moveResourceDTO);

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
        #endregion
    }
}
