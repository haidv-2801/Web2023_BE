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

namespace Web2023_BE.Infrastructure
{
    /// <summary>
    /// Repository danh mục bài viết
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        #region Constructer
        public PostRepository(IConfiguration configuration) : base(configuration)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Lấy danh sách bài viết phân trang, tìm kiếm
        /// </summary>
        /// <param name="filterValue">Giá trị tìm kiếm</param>
        /// <param name="limit">Số bản ghi trên 1 trang</param>
        /// <param name="offset">Số trang</param>
        /// <returns>Danh sách bài viết</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        public DbResponse GetPostsFilterPaging(string filterValue, int? pageSize = null, int? pageNum = null)
        {
            //1. Ánh xạ giá trị
            var serviceResult = new DbResponse();
            var param = new DynamicParameters();
            param.Add($"@v_filter", filterValue);
            param.Add($"@v_page_size", pageSize);
            param.Add($"@v_page_number", pageNum);
            param.Add($"@v_total_record", dbType: DbType.Int32, direction: ParameterDirection.Output);

            //2. Tạo kết nối và truy vấn                        
            var posts = _dbConnection.Query<Post>($"Proc_Get{_tableName}sFilterPaging", param: param, commandType: CommandType.StoredProcedure);

            serviceResult.Data = (IEnumerable<Post>)posts;
            serviceResult.TotalRecords = param.Get<int>("v_total_record");

            //3. Trả về dữ liệu
            return serviceResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterValue"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public DbResponse GetPostsByMenuID(Guid MenuID)
        {
            //1. Ánh xạ giá trị
            var serviceResult = new DbResponse();
            var param = new DynamicParameters();
            param.Add($"@v_menuid", MenuID);
            param.Add($"@v_total_record", dbType: DbType.Int32, direction: ParameterDirection.Output);

            //2. Tạo kết nối và truy vấn                        
            var posts = _dbConnection.Query<Post>($"Proc_Get{_tableName}sByMenuID", param: param, commandType: CommandType.StoredProcedure);

            serviceResult.Data = (IEnumerable<Post>)posts;
            serviceResult.TotalRecords = param.Get<int>("v_total_record");

            //3. Trả về dữ liệu
            return serviceResult;
        }
        #endregion
    }
}
