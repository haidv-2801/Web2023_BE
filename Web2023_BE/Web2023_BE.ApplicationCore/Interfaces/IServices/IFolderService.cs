using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public interface IFolderService : IBaseService<Folder>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveResourceDTO"></param>
        /// <returns></returns>
        Task<ServiceResult> MoveFolderAsync(MoveResourceDTO moveResourceDTO);

    }
}
