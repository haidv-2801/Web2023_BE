using Web2023_BE.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Entities
{
    /// <summary>
    /// Trạng thái bản ghi
    /// Sử dụng để cho form chi tiết để xác định bản ghi là thêm/sửa/xóa
    /// </summary>
    public interface IRecordState
    {
        /// <summary>
        /// Trạng thái của bản ghi
        /// </summary>
        ModelState State { get; set; }
    }
}
