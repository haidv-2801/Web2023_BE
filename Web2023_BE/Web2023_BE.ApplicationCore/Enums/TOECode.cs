using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Entities
{
    public enum TOECode
    {
        /// <summary>
        /// Hợp lệ
        /// </summary>
        Valid = 100,

        /// <summary>
        /// Không hợp lệ
        /// </summary>
        InValid = 200,

        /// <summary>
        /// Thành công  
        /// </summary>
        Success = 900,

        /// <summary>
        /// Thất bại  
        /// </summary>
        Fail = 700,

        /// <summary>
        /// Lỗi exception
        /// </summary>
        Exception = 500
    }


}
