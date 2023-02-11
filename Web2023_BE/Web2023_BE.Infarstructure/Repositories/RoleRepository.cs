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
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        #region Constructer
        public RoleRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        #region Methods
        #endregion
    }
}
