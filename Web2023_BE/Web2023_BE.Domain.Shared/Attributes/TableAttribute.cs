using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Attributes
{
    /// <summary>
    /// Attribute đánh dấu tên bảng
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Tên bảng chi tiết
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="table">Tên bảng</param>
        public TableAttribute(string table)
        {
            this.Table = table;
        }
    }
}
