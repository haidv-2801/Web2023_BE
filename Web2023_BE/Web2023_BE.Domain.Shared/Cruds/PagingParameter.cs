using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Legder.Domain.Shared.Cruds
{
    /// <summary>
    /// Tham số request paging
    /// </summary>
    public class PagingParameter
    {
        /// <summary>
        /// Bỏ qua bao nhiêu bản ghi
        /// </summary>
        public int skip { get; set; }
        /// <summary>
        /// Lấy bao nhiêu bản ghi
        /// </summary>
        public int take { get; set; }
        /// <summary>
        /// Điều kiện sắp xếp
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// Tham số load dữ liệu
        /// dictionary string json
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// Điều kiện lọc custom
        /// </summary>
        public string filter { get; set; }
        /// <summary>
        /// Danh sách các cột trả về
        /// </summary>
        public string columns { get; set; }
    }

    /// <summary>
    /// Tham số request paging
    /// </summary>
    public class PagingSummaryParameter
    {
        /// <summary>
        /// Tham số load dữ liệu
        /// dictionary string json
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// Điều kiện lọc custom
        /// </summary>
        public string filter { get; set; }
        /// <summary>
        /// Danh sách các cột trả về
        /// </summary>
        public string columns { get; set; }
    }
}
