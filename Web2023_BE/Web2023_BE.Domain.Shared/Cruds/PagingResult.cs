using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Cruds
{
    /// <summary>
    /// Chứa các thông tin trả về dạng paging
    /// </summary>
    public class PagingResult
    {
        /// <summary>
        /// Dữ liệu trang
        /// </summary>
        public IList Data { get; set; }
        /// <summary>
        /// Bảng không có dữ liệu
        /// </summary>
        public bool Empty { get; set; }
        ///// <summary>
        ///// Thời gian xử lý
        ///// </summary>
        //public Dictionary<string, TimeSpan> Times { get; set; }
    }

    /// <summary>
    /// Chứa các thông tin trả về dạng paging
    /// </summary>
    public class PagingSummaryResult
    {
        /// <summary>
        /// Tổng bản ghi
        /// </summary>
        public long Total { get; set; }
        /// <summary>
        /// Dữ liệu summary
        /// </summary>
        public object Data { get; set; }
        ///// <summary>
        ///// Thời gian xử lý
        ///// </summary>
        //public Dictionary<string, TimeSpan> Times { get; set; }
    }
}
