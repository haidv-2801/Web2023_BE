using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Helpers;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Interfaces.IServices;

namespace Web2023_BE.ApplicationCore
{
    public class ImageManagerService : BaseService<ImageManager>, IImageManagerService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly IHttpContextAccessor _contextAccessor;

        public ImageManagerService(IBaseRepository<ImageManager> baseRepository, IWebHostEnvironment hostEnvironment, IHttpContextAccessor contextAccessor) : base(baseRepository)
        {
            _hostEnvironment = hostEnvironment;
            _contextAccessor = contextAccessor;
        }

        public async Task<ServiceResult> CreateImage(ImageManagerDTO imageManager)
        {
            imageManager.ImageName = await SaveImage(imageManager.ImageFile);


            var imagemanagerNew = new ImageManager()
            {
                ImageID = Guid.NewGuid(),
                ImageName = imageManager.ImageName,
                Url = imageManager.Url + imageManager.ImageName,
                FolderID = imageManager.FolderID,
                CreatedBy = _contextAccessor.HttpContext.Items["user"] + "" ?? "",
                ModifiedBy = _contextAccessor.HttpContext.Items["user"] + "" ?? ""

            };
            var result = await Insert(imagemanagerNew);

            return result;
        }


        public async Task<ServiceResult> DeleteImage(string id)
        {
            var guidID = Guid.Parse(id);
            var item = await GetEntityById(guidID);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", item.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);


            var result = Delete(guidID);

            return result;
        }


        public async Task<ServiceResult> UpdateImage(string id, ImageManagerDTO imageManager)
        {
            if (imageManager.ImageName != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageManager.ImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                imageManager.ImageName = await SaveImage(imageManager.ImageFile);
                imageManager.Url = imageManager.Url + imageManager.ImageName;
            }
            var guidID = Guid.Parse(id);
            var entity = await GetEntityById(guidID);
            var result = await Update(guidID, entity);

            return result;
        }




        public async Task<ServiceResult> GetListImagePagingAsync(PagingRequest pagingRequest)
        {
            var serviceResult = new ServiceResult();
            var pagingRequestFolder = new PagingRequest()
            {
                Filter = pagingRequest.CustomParams.FilterFolder,
                Sort = pagingRequest.CustomParams.SortFolder,
                PageIndex = pagingRequest.PageIndex,
                PageSize = pagingRequest.PageSize
            };
            var dataFolder = new List<Folder>();
            var data = new List<ImageManager>();
            var surplus = 0;

            var countFolder = await CountTotalRecordByClause(pagingRequestFolder, "folder");

            var countFilter = await CountTotalRecordByClause(pagingRequest, "image_manager");
            surplus = countFolder - (countFolder / pagingRequest.PageSize) * pagingRequest.PageSize;
            var isCheckGetFolder = countFolder + pagingRequest.PageSize - pagingRequest.PageIndex * pagingRequest.PageSize;
            if (countFolder > 0 && isCheckGetFolder > 0)
            {
                var resultFolder = await GetEntitiesFilter<Folder>(pagingRequestFolder, "folder");
                var pagingResponse = FunctionHelper.Deserialize<PagingResponse>(FunctionHelper.Serialize<object>(resultFolder.Data));
                dataFolder = FunctionHelper.Deserialize<List<Folder>>(FunctionHelper.Serialize<object>(pagingResponse.PageData));
            }

            if (surplus > 0)
            {
                pagingRequest.PageIndex = pagingRequest.PageIndex - countFolder / pagingRequest.PageSize;

                if (pagingRequest.PageIndex > 1)
                {
                    pagingRequest.Delta = pagingRequest.PageSize - surplus;
                }


                var resultData = await GetEntitiesFilter(pagingRequest, "image_manager");
                var pagingResponse = FunctionHelper.Deserialize<PagingResponse>(FunctionHelper.Serialize<object>(resultData.Data));
                data = FunctionHelper.Deserialize<List<ImageManager>>(FunctionHelper.Serialize<object>(pagingResponse.PageData));
            }


            var resultDictionary = new Dictionary<string, object>();

            resultDictionary.Add("CustomData", dataFolder);
            resultDictionary.Add("PageData", data);
            resultDictionary.Add("Total", countFolder + countFilter);
            serviceResult.Data = resultDictionary;
            return serviceResult;
        }


        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }




    }
}