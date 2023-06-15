using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Entities
{
    /// <summary>
    /// Xử lý thông tin version của dữ liệu
    /// </summary>
    public interface IRecordVersion
    {
        /// <summary>
        /// Version của dữ liệu
        /// Mặc định sẽ đọc thông tin từ trường ModifiedDate
        /// </summary>        
        long EditVersion { get; set; }
    }
}
