using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface danh mục menu
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        /// <summary>
        /// Lấy danh sách menu phân trang, tìm kiếm
        /// </summary>
        /// <param name="filterValue">Giá trị tìm kiếm</param>
        /// <param name="limit">Số bản ghi trên 1 trang</param>
        /// <param name="offset">Số trang</param>
        /// <returns>Danh sách menu</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        DbResponse GetMenusFilterPaging(string filterValue, int? pageSize = null, int? pageNum = null);

        /// <summary>
        /// Lấy danh sách menu và tổng số bài viết trong menu đó
        /// </summary>
        /// <returns></returns>
        DbResponse GetMenusAndPostsCount();
    }
}
