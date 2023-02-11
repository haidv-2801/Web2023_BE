using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Attributes
{
    // <summary>
    /// Attribute đánh dấu các dữ liệu detail của master
    /// Kiểu dữ liệu của thuộc tính gắn attribute này bắt buộc phải là list object để cho tiện xử lý, đỡ phải case cho code đơn giản và tốt cho hiệu năng
    /// </summary>
    public class DetailAttribute : Attribute
    {
        /// <summary>
        /// Tên trường maping với master key
        /// </summary>
        public string MasterKeyField { get; set; }
        /// <summary>
        /// Kiểu dữ liệu
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="masterKeyField">Tên trường maping với master key</param>
        /// <param name="type">Kiểu dữ liệu</param>
        public DetailAttribute(string masterKeyField, Type type)
        {
            this.MasterKeyField = masterKeyField;
            this.Type = type;
        }
    }
}
