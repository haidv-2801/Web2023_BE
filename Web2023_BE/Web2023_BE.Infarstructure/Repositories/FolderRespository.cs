using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Interfaces;

namespace Web2023_BE.Infarstructure
{
    public class FolderRespository : BaseRepository<Folder>, IFolderRepository
    {
        public FolderRespository(IConfiguration configuration) : base(configuration)
        {
        }


    }
}
