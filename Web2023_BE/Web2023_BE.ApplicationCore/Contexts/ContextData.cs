using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore
{
    public class ContextData
    {
        /// <summary>
        /// Nếu request cookie có thông tin này thì phải khớp mới hợp lệ
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// Tài khoản nào
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Dữ liệu nào
        /// </summary>
        public int DatabaseId { get; set; }
    }
}
