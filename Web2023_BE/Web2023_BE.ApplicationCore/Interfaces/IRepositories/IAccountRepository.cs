using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface danh mục menu
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public interface IAccountRepository : IBaseRepository<Account>
    {
        /// <summary>
        /// Lấy danh sách menu phân trang, tìm kiếm
        /// </summary>
        /// <param name="filterValue">Giá trị tìm kiếm</param>
        /// <param name="limit">Số bản ghi trên 1 trang</param>
        /// <param name="offset">Số trang</param>
        /// <returns>Danh sách menu</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        DbResponse GetAccountsFilterPaging(string filterValue, int? pageSize = null, int? pageNum = null);

        int UpdateAccountPassword(Guid entityId, AccountPasswordChangeDTO entity);

        Task<Account> Login(AccountLoginDTO account);
        
        Task<List<Role>> GetRolesByAccountID(string AccountID);
    }
}
