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

namespace Web2023_BE.Infrastructure
{
    /// <summary>
    /// Repository danh mục menu
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        #region Constructer
        public MenuRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public DbResponse GetMenusAndPostsCount()
        {
            //1. Ánh xạ giá trị
            var serviceResult = new DbResponse();

            //2. Tạo kết nối và truy vấn                        
            var posts = _dbConnection.Query<object>("select * from view_countpostbymenuid",  commandType: CommandType.Text);

            serviceResult.Data = (IEnumerable<object>)posts;

            //3. Trả về dữ liệu
            return serviceResult;
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
        public DbResponse GetMenusFilterPaging(string filterValue, int? pageSize = null, int? pageNum = null)
        {
            //1. Ánh xạ giá trị
            var serviceResult = new DbResponse();
            var param = new DynamicParameters();
            param.Add($"@v_filter", filterValue);
            param.Add($"@v_page_size", pageSize);
            param.Add($"@v_page_number", pageNum);
            param.Add($"@v_total_record", dbType: DbType.Int32, direction: ParameterDirection.Output);

            //2. Tạo kết nối và truy vấn                        
            var posts = _dbConnection.Query<Menu>($"Proc_Get{_tableName}sFilterPaging", param: param, commandType: CommandType.StoredProcedure);

            serviceResult.Data = (IEnumerable<Menu>)posts;
            serviceResult.TotalRecords = param.Get<int>("v_total_record");

            //3. Trả về dữ liệu
            return serviceResult;
        }
        #endregion
    }
}
