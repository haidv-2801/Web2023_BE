using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
{
    /// <summary>
    /// Thực thể exportcolumn
    /// </summary>
    public class ExportColumn
    {
        /// <summary>
        /// Mã cột
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Tên cộct
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Tên hiển thị của cột
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Chiều rộng
        /// </summary>
        public int Width { get; set; }
    }
}
