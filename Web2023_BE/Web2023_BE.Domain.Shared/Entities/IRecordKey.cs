using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Entities
{
    /// <summary>
    /// Quy định khóa chính của bản ghi luông
    /// </summary>
    public interface IRecordKey<TKey>
    {
        TKey Id { get; set; }
    }
}
