using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Legder.Domain.Configs
{
    /// <summary>
    /// Cấu hình router redirect
    /// </summary>
    public class RouterConfig
    {
        /// <summary>
        /// Mặc định môi trường - khi không maping dc môi trường nào khai báo thì sẽ lấy theo môi trường này
        /// </summary>
        public string DefaultEnv { get; set; }
        /// <summary>
        /// Danh sách các môi trường đang hosting
        /// Key: Version ứng dụng, vd: 1.0.0.56
        ///     Giá trị trường env trong database sẽ lưu tương ứng
        /// Value: tên env theo config Envs
        /// </summary>
        public Dictionary<string, string> VersionMapping { get; set; }
        /// <summary>
        /// Cấu hình địa chỉ internal cho từng môi trường
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> ApiUrl { get; set; }
        /// <summary>
        /// key maping từ request đọc ra thông tin môi trường
        /// </summary>
        public Dictionary<string, List<string>> MapingKey { get; set; }
        /// <summary>
        /// Thứ tự kiểm tra các key
        /// </summary>
        public List<string> MapingKeyOrder { get; set; }
    }
}
