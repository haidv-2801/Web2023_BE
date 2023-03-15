using System;
using System.Collections.Generic;
using System.Text;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Interfaces.IServices;

namespace Web2023_BE.ApplicationCore.Services
{
    public class FolderService : BaseService<Folder>, IFolderService
    {
        public FolderService(IBaseRepository<Folder> baseRepository) : base(baseRepository)
        {
        }
    }
}
