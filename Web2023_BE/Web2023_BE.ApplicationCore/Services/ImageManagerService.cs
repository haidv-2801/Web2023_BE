﻿using System;
using System.Collections.Generic;
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
        public ImageManagerService(IBaseRepository<ImageManager> baseRepository) : base(baseRepository)
        {
        }

        public async Task<ServiceResult> GetListImagePagingAsync(PagingRequest pagingRequest)
        {
            var serviceResult = new ServiceResult();
            var pagingRequestFolder = new PagingRequest()
            {
                Filter = pagingRequest.CustomParams.FilterFolder,
                Sort = pagingRequest.CustomParams.FilterFolder

            };
            var dataFolder = new List<Folder>();
            var data = new List<ImageManager>();
            var surplus = 0;

            var countFolder = await CountTotalRecordByClause(pagingRequestFolder, "folder");

            var countFilter = await CountTotalRecordByClause(pagingRequest, "image_manager");

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