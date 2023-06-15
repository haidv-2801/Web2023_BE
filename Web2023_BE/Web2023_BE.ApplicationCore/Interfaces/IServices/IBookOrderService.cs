using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public interface IBookOrderService : IBaseService<BookOrder>
    {
        Task<ServiceResult> InsertBookOrder(BookOrder bookOrder);

        Task<string> GetNextBookOrderCode();

        Task<long> GetTotalBookOrdered();

        Task<IEnumerable<BookOrderTopReportDTO>> TopBookBorrowed();

        Task<long> CountTotalBookUserLoanByOrderStatus(List<string> orderStatus, Guid accountID);
    }
}
