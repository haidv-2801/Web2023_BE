using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Helpers;
using Web2023_BE.ApplicationCore.Interfaces;

namespace Web2023_BE.ApplicationCore
{
    public class ImageManagerService : BaseService<ImageManager>, IImageManagerService
    {
        private readonly IStorageService _storageService;
        public ImageManagerService(IBaseRepository<ImageManager> baseRepository, IStorageService storageService) : base(baseRepository)
        {
            _storageService = storageService;
        }

        public async Task<bool> DeleteImage(string id , StorageFileType type, string name)
        {
            var result = true;
            try
            {
                await _storageService.DeleteAsync(type, name);
                await DeleteAsync(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                return result = false;
            }
            return result;
        }

        public async Task<ServiceResult> GetListImagePagingAsync(PagingRequest pagingRequest)
        {
            var serviceResult = new  ServiceResult();
            var pagingRequestFolder = new PagingRequest()
            {
                Filter = pagingRequest.CustomParams.FilterFolder,
                Sort = pagingRequest.CustomParams.FilterFolder

            };
            var dataFolder = new List<Folder>();
            var data = new List<ImageManager>();
            var surplus = 0;

            var countFolder = await CountTotalRecordByClause(pagingRequestFolder, "folder");

            var countFilter = await CountTotalRecordByClause(pagingRequestFolder, "image_manager");

            if (countFolder > 0)
            {
                surplus = countFolder % pagingRequest.PageSize;
                var resultFolder = await GetEntitiesFilter(pagingRequestFolder, "folder");
                dataFolder = FunctionHelper.Deserialize<List<Folder>>(FunctionHelper.Serialize<object>(resultFolder.Data));
            }

            if (surplus > 0 && surplus > pagingRequest.PageIndex)
            {
                pagingRequest.Delta = pagingRequest.PageSize - surplus;
                var resultData = await GetEntitiesFilter(pagingRequestFolder, "folder");
                data = FunctionHelper.Deserialize<List<ImageManager>>(FunctionHelper.Serialize<object>(resultData.Data));
            }
            else
            {
                var resultData = await GetEntitiesFilter(pagingRequestFolder, "folder");
                data = FunctionHelper.Deserialize<List<ImageManager>>(FunctionHelper.Serialize<object>(resultData.Data));
            }

            var resultDictionary = new Dictionary<string, object>();

            resultDictionary.Add("CustomData", dataFolder);
            resultDictionary.Add("PageData", data);
            resultDictionary.Add("Total", countFolder + countFilter);
            serviceResult.Data = resultDictionary;
            return serviceResult;
        }


    }
}
