using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Enums
{
    /// <summary>
    /// Trạng thái của model
    /// </summary>
    public enum ModelState
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
}
