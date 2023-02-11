using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Legder.Domain.Shared.Cruds
{
    public class SaveParameter<T> : CrudParameter
    {
        /// <summary>
        /// Dữ liệu cất
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Trả về dữ liệu sau khi cất
        /// Dùng cho các client lấy data update lại UI
        /// </summary>
        public bool ReturnRecord { get; set; }
    }
}
