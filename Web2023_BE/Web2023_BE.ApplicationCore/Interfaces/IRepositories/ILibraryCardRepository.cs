using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public interface ILibraryCardRepository : IBaseRepository<LibraryCard>
    {
        Task<long> GetTotalLibraryCard();

        Task<LibraryCard> GetLibraryCardByAccountID(Guid accountID);

        Task<string> GetNextCardCode();

        Task<int> AcceptMany(string idsText, int cardStatus);
    }
}
