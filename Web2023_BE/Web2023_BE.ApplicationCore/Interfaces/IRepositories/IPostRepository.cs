using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface danh mục bài viết
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public interface IPostRepository : IBaseRepository<Post>
    {
        /// <summary>
        /// Lấy danh sách bài viết phân trang, tìm kiếm
        /// </summary>
        /// <param name="filterValue">Giá trị tìm kiếm</param>
        /// <param name="limit">Số bản ghi trên 1 trang</param>
        /// <param name="offset">Số trang</param>
        /// <returns>Danh sách bài viết</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        DbResponse GetPostsFilterPaging(string filterValue, int? pageSize = null, int? pageNum = null);
    }
}
