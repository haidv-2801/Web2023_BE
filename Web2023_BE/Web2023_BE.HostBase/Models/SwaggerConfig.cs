using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web2023_BE.HostBase
{
    /// <summary>
    /// Cấu hình Swagger
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Đường dẫn file json
        /// </summary>
        public string JsonPath { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; } = "My API V1";
        public string Version { get; set; } = "v1";
        public string Title { get; set; } = "Api";
    }
}