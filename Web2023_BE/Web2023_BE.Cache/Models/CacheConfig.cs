using System;
using System.Collections.Generic;
using System.Text;


namespace Web2023_BE.Cache
{
    /// <summary>
    /// Cấu hình cache
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// Phân chia các nhóm cache dist
        /// </summary>
        public List<string> DistGroups { get; set; }
        /// <summary>
        /// Danh sách các item cache
        /// </summary>
        public Dictionary<string, CacheItemConfig> Items { get; set; }
    }
}
