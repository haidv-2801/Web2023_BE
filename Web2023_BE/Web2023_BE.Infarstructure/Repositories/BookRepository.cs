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
using System.Threading.Tasks;

namespace Web2023_BE.Infrastructure
{
    /// <summary>
    /// Repository danh mục bài viết
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public class BookRepository : BaseRepository<BookItem>, IBookRepository
    {
        #region Constructer
        public BookRepository(IConfiguration configuration) : base(configuration)
        {

        }

        #endregion

        #region Method
        public async Task<List<BookItem>> GetBooksByIDs(string ids)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@v_bookids", ids, DbType.String);
            return (await _dbConnection.QueryAsync<BookItem>("Proc_GetBooksByIDs", param: dynamicParams, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<string> GetNextBookCode() => await _dbConnection.QueryFirstOrDefaultAsync<string>("Proc_NextBookCode", commandType: CommandType.StoredProcedure);

        public async Task<long> GetTotalBook() => await _dbConnection.QueryFirstOrDefaultAsync<long>("Proc_GetTotalBook", commandType: CommandType.StoredProcedure);
        #endregion
    }
}
