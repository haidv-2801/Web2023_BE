using Web2023_BE.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Entities;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Extensions;

namespace Web2023_BE.Infrastructure
{
    /// <summary>
    /// Repository danh mục menu
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        #region Constructer
        public AccountRepository(IConfiguration configuration) : base(configuration)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Lấy danh sách menu phân trang, tìm kiếm
        /// </summary>
        /// <param name="filterValue">Giá trị tìm kiếm</param>
        /// <param name="limit">Số bản ghi trên 1 trang</param>
        /// <param name="offset">Số trang</param>
        /// <returns>Danh sách menu</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        public DbResponse GetAccountsFilterPaging(string filterValue, int? pageSize = null, int? pageNum = null)
        {
            //1. Ánh xạ giá trị
            var serviceResult = new DbResponse();
            var param = new DynamicParameters();
            param.Add($"@v_filter", filterValue);
            param.Add($"@v_page_size", pageSize);
            param.Add($"@v_page_number", pageNum);
            param.Add($"@v_total_record", dbType: DbType.Int32, direction: ParameterDirection.Output);

            //2. Tạo kết nối và truy vấn                        
            var posts = _dbConnection.Query<AccountDTO>($"Proc_Get{_tableName}sFilterPaging", param: param, commandType: CommandType.StoredProcedure);

            serviceResult.Data = (IEnumerable<AccountDTO>)posts;
            serviceResult.TotalRecords = param.Get<int>("v_total_record");

            //3. Trả về dữ liệu
            return serviceResult;
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateAccountPassword(Guid entityId, AccountPasswordChangeDTO entity)
        {
            var rowAffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    //1. Ánh xạ giá trị id
                    var keyName = _modelType.GetKeyName();
                    entity.GetType().GetProperty(keyName).SetValue(entity, entityId);

                    //2. Duyệt các thuộc tính trên customer và tạo parameters
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add($"@v_{keyName}", entityId);
                    dynamicParams.Add($"@v_password", entity.Password);
                    dynamicParams.Add($"@v_modifieddate", entity.ModifiedDate);
                    dynamicParams.Add($"@v_modifiedby", entity.ModifiedBy);

                    //3. Kết nối tới CSDL:
                    rowAffects = _dbConnection.Execute($"Proc_Update{_tableName}Password", param: dynamicParams, transaction: transaction, commandType: CommandType.StoredProcedure);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            //4. Trả về dữ liệu
            return rowAffects;
        }

        public async Task<Account> Login(AccountLoginDTO account)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add($"@v_email", account.Email);
            dynamicParams.Add($"@v_password", account.Password);
            var accounts = await _dbConnection.QueryFirstOrDefaultAsync<Account>("Proc_LoginAccount", param: dynamicParams, commandType: CommandType.StoredProcedure);
            return accounts;
        }

        public async Task<List<Role>> GetRolesByAccountID(string AccountID)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add($"@v_accountid", AccountID);
            var roles = await _dbConnection.QueryAsync<Role>("Proc_GetRoleByAccountID", param: dynamicParams, commandType: CommandType.StoredProcedure);
            return roles.ToList();
        }
        #endregion
    }
}
