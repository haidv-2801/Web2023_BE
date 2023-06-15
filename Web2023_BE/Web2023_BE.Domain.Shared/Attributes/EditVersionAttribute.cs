using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Attributes
{
    /// <summary>
    /// Attribute Đánh dấu trường dữ liệu sẽ xử lý version
    /// </summary>
    public class EditVersionAttribute : Attribute
    {
        /// <summary>
        /// Tên trường dữ liệu gốc
        /// </summary>
        public string DataField { get; set; } = "Modified";
    }
}
