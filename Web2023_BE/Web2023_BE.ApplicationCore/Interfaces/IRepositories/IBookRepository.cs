using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface danh mục bài viết
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public interface IBookRepository : IBaseRepository<BookItem>
    {
        Task<string> GetNextBookCode();

        Task<long> GetTotalBook();

        Task<List<BookItem>> GetBooksByIDs(string ids);
    }
}
