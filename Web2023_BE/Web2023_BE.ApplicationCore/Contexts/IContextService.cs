using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore
{
    /// <summary>
    /// Phiên làm việc
    /// Dùng để khởi tạo 1 số thông tin: connection kết nối tới db, thông tin user đăng nhập...
    /// </summary>
    public interface IContextService
    {
        void SetContext(ContextData contextData);
        ContextData GetContext();
    }
}
