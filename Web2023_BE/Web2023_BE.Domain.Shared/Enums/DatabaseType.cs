using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Enums
{
    /// <summary>
    /// Loại dữ liệu
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Dữ liệu master
        /// </summary>
        Master = 0,
        /// <summary>
        /// Dữ liệu riêng từng công ty/dữ liệu
        /// </summary>
        Tenant = 1,
    }
}
