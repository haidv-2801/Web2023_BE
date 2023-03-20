using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Entities
{
    public enum Enums
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

    public enum EntityState
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }

    public enum HtmlSectionType
    {
        Footer = 1,
    }

    public enum PostType
    {
        None = 1,
        Recruitment = 2, // tuyển dụng
    }
}
