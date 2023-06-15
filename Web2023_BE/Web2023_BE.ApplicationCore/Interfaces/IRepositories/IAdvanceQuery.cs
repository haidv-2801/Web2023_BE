using System.Collections.Generic;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public interface IAdvanceQuery
    {
        Task<CommonStats> StatsAggregationAsync(string key, string fieldName);
        /// <summary>
        /// This method is just an example how to pull individual stats though all stats can be pull 
        /// using above StatsAggregationAsync method.
        /// </summary>
        Task<double> MedianAggregationAsync(string key, string fieldName);
        Task<List<GroupStats>> GroupByAsync(string fieldName);
    }
}
