using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Enums;
using Web2023_BE.ApplicationCore.Interfaces;

namespace Web2023_BE.ApplicationCore
{
    public class FolderService : BaseService<Folder>, IFolderService
    {

        #region Declare
        private readonly IFolderRepository _folderRepository;
        private readonly ImageManagerService _imageManagerService;

        IConfiguration _config;
        #endregion
        public FolderService(IFolderRepository folderRepository, ImageManagerService imageManagerService,IConfiguration config) : base(folderRepository)
        {
            _folderRepository = folderRepository;
            _imageManagerService = imageManagerService; 
        }

        public async Task<ServiceResult> MoveFolderAsync(MoveResourceDTO moveResourceDTO)
        {
            var result = new ServiceResult();
            var dataResource = new object();

            var destinationFolder = await GetEntityById(Guid.Parse(moveResourceDTO.DestinationFolderID));
            switch (moveResourceDTO.ModuleType)
            {
                case ModuleType.IMAGE:
                    dataResource = (await _imageManagerService.GetListImagePagingAsync(moveResourceDTO.PagingRequest)).Data;

                    // code block
                    break;
                default:
                    break;
               
            }
            //todo


            return result;
        }



    }
}
