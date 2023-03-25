using Microsoft.AspNetCore.Http;
using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public interface IBaseService<TEntity>
    {

        /// <summary>
        /// Lấy danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        Task<IEnumerable<TEntity>> GetEntities();

        /// <summary>
        /// Lấy danh sách bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        Task<ServiceResult> GetEntitiesFilter(PagingRequest pagingRequest, string viewOrTableName = "");
        Task<ServiceResult> GetEntitiesFilter<T>(PagingRequest pagingRequest, string viewOrTableName = "");

        /// <summary>
        ///  Lấy bản ghi theo id
        /// </summary>
        /// <param name="entityId">Id của bản ghi</param>
        /// <returns>Bản ghi thông tin 1 bản ghi</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        Task<TEntity> GetEntityById(Guid entityId);

        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>Số bản ghi</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        Task<ServiceResult> Insert(TEntity entity);

        /// <summary>
        /// Cập nhập thông tin bản ghi
        /// </summary>
        /// <param name="entityId">Id bản ghi</param>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        Task<ServiceResult> Update(Guid entityId, TEntity entity);

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Số dòng bị xóa</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        ServiceResult Delete(Guid entityId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="viewOrTableName"></param>
        /// <returns></returns>
        Task<int> CountTotalRecordByClause(PagingRequest pagingRequest, string viewOrTableName = "");

        /// <summary>
        /// Đọc file excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// CREATED BY: DVHAI (07/07/2021)
        //Task<ServiceResult> readExcelFile(IFormFile formFile, CancellationToken cancellationToken);

        /// <summary>
        /// Thêm nhiều bản ghi
        /// </summary>
        /// <param name="ieEntities"></param>
        /// <returns></returns>\
        /// CREATED BY: DVHAI (07/07/2021)
        //ServiceResult MultiInsert(IEnumerable<TEntity> ieEntities);
    }
}
