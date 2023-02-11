using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
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
    }
}
