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
    /// Repository danh mục menu
    /// </summary>
    /// CREATED BY: DVHAI (07/07/2021)
    public class LibraryCardRepository : BaseRepository<LibraryCard>, ILibraryCardRepository
    {

        #region Constructer
        public LibraryCardRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        #region Methods
        public async Task<long> GetTotalLibraryCard() => await _dbConnection.QueryFirstOrDefaultAsync<long>("Proc_GetTotalMemberCard", commandType: CommandType.StoredProcedure);

        public async Task<LibraryCard> GetLibraryCardByAccountID(Guid accountID)
        {
            var parameter = new DynamicParameters();
            parameter.Add("v_AccountID", accountID.ToString());
            return await _dbConnection.QueryFirstOrDefaultAsync<LibraryCard>("Proc_GetLibraryCardByAccountID", param: parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<string> GetNextCardCode() => await _dbConnection.QueryFirstOrDefaultAsync<string>("Proc_GetNextCardCode", commandType: CommandType.StoredProcedure);

        public async Task<int> AcceptMany(string idsText, int cardStatus)
        {
            var parameter = new DynamicParameters();
            parameter.Add("v_adstext", idsText);
            parameter.Add("v_CardStatus", cardStatus);
            return await _dbConnection.QueryFirstOrDefaultAsync<int>("Proc_AcceptMany", param: parameter, commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
