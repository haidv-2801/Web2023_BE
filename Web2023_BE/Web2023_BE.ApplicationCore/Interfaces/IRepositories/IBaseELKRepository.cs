using System.Threading.Tasks;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public interface IBaseELKRepository
    {
        int NumberOfShards { get; set; }
        int NumberOfReplicas { get; set; }
        Task<long> GetDocumentsCount();
        Task<bool> IsIndexExists();
        Task RefreshIndex();
        Task DeleteIndexAsync(string indexName); 
    }
}
