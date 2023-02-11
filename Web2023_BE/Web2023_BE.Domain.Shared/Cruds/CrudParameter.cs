using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Legder.Domain.Shared.Cruds
{
    public class CrudParameter
    {
        /// <summary>
        /// Danh sách các rule validate bỏ qua
        /// </summary>
        public List<string> Ignores { get; set; }

        /// <summary>
        /// Kiểm tra có bỏ qua kiểm tra rule validate này không
        /// </summary>
        /// <param name="code">mã rule</param>
        public bool CheckIgnore(string code)
        {
            if (this.Ignores != null && !string.IsNullOrEmpty(code))
            {
                foreach (var item in this.Ignores)
                {
                    if (code.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
