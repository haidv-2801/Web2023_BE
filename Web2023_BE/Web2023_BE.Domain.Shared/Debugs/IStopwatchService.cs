using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web2023_BE.Domain.Shared.Debugs
{
    /// <summary>
    /// Lớp debug thời gian xử lý
    /// Dùng để tách ra kiểm tra xem chậm ở đâu để xử lý
    /// </summary>
    public interface IStopwatchService
    {
        /// <summary>
        /// Đọc kết quả
        /// </summary>
        object GetResult();

        /// <summary>
        /// Ghi lại thời gian xử lý hành động
        /// </summary>
        /// <param name="actionKey">key phân biệt các action với nhau</param>
        /// <param name="action">Hàm xử lý</param>
        void Capture(string actionKey, Action action);

        /// <summary>
        /// Ghi lại thời gian xử lý hành động
        /// </summary>
        /// <param name="times">Dictionary lưu kết quả</param>
        /// <param name="actionKey">key phân biệt các action với nhau</param>
        /// <param name="action">Hàm xử lý</param>
        Task Capture(string actionKey, Func<Task> action);

        /// <summary>
        /// Ghi lại thời gian xử lý hành động
        /// </summary>
        /// <param name="times">Dictionary lưu kết quả</param>
        /// <param name="actionKey">key phân biệt các action với nhau</param>
        /// <param name="action">Hàm xử lý</param>
        T Capture<T>(string actionKey, Func<T> action);

        /// <summary>
        /// Ghi lại thời gian xử lý hành động
        /// </summary>
        /// <param name="times">Dictionary lưu kết quả</param>
        /// <param name="actionKey">key phân biệt các action với nhau</param>
        /// <param name="action">Hàm xử lý</param>
        Task<T> Capture<T>(string actionKey, Func<Task<T>> action);
    }
}
