using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Interfaces;

namespace Web2023_BE.Infarstructure.Repositories
{
    public class ImageManagerRepository : BaseRepository<ImageManager>, IImageManagerRepository
    {
        public ImageManagerRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
