using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore.Interfaces
{
  public interface IAccountService : IBaseService<Account>
  {
    /// <summary>
    /// Lấy danh sách nhân viên phân trang, tìm kiếm
    /// </summary>
    /// <param name="filterValue">Giá trị tìm kiếm</param>
    /// <param name="limit">Số bản ghi trên 1 trang</param>
    /// <param name="offset">Số trang</param>
    /// <returns>Danh sách bài đăng</returns>
    /// CREATED BY: DVHAI (07/07/2021)
    ServiceResult GetAccountsFilterPaging(string filterValue, int limit, int offset);

    /// <summary>
    /// Đăng nhập
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    Task<object> Login(AccountLoginDTO userRequest);

    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    Task<ServiceResult> ChangePassword(Guid entityId, AccountPasswordChangeDTO entity);
  }
}
