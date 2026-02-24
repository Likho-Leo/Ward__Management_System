using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IStatisticsRepository
    {
        Task<DashboardViewModel> GetStatistics();
        Task<double> GetAverageBedsPerWard(); // Update the method name
        Task<List<ConditionStats>> GetConditionsStats();
        Task<int> GetTotalActiveUsers();
        Task<int> GetTotalInactiveUsers();
    }
}
