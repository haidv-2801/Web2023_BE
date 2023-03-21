using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;

namespace Web2023_BE.ApplicationCore.Interfaces.IServices
{
    public interface IImageManagerService : IBaseService<ImageManager>
    {
        Task<ServiceResult> GetListImagePagingAsync(PagingRequest pagingRequest);

        Task<ServiceResult> CreateImage(ImageManagerDTO imageManager);
        Task<ServiceResult> DeleteImage(string id);
        Task<ServiceResult> UpdateImage(string id, ImageManagerDTO imageManager);

    }
}