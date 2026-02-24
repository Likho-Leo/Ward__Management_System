using Dapper;
using System.Data;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly IDbConnection _dbConnection;

        public StatisticsRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<DashboardViewModel> GetStatistics()
        {
            var model = new DashboardViewModel
            {
                TotalHospitals = await GetTotalHospitals(),
                TotalBeds = await GetTotalBeds(),
                TotalConditions = await GetTotalConditions(),
                TotalConsumables = await GetTotalConsumables(),
                TotalMedications = await GetTotalMedications(),
                TotalUsers = await GetTotalUsers(),
                TotalWards = await GetTotalWards(),
                AverageBedsPerWard = await GetAverageBedsPerWard(), // Update this line
                Conditions = await GetConditionsStats(),
                TotalActiveUsers = await GetTotalActiveUsers(),
                TotalInactiveUsers = await GetTotalInactiveUsers()
            };

            return model;
        }

        private async Task<int> GetTotalHospitals()
        {
            var query = "SELECT COUNT(*) FROM HospitalInfo WHERE InActive = 'N'"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        private async Task<int> GetTotalBeds()
        {
            var query = "SELECT COUNT(*) FROM Bed WHERE InActive = 'N'"; // Ensure this table name is correct
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        private async Task<int> GetTotalConditions()
        {
            var query = "SELECT COUNT(*) FROM Condition WHERE InActive = 'N'"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        private async Task<int> GetTotalConsumables()
        {
            var query = "SELECT COUNT(*) FROM Consumable WHERE InActive = 'N'"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        private async Task<int> GetTotalMedications()
        {
            var query = "SELECT COUNT(*) FROM Medication WHERE InActive = 'N'"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        private async Task<int> GetTotalUsers()
        {
            var query = "SELECT COUNT(*) FROM Users"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        private async Task<int> GetTotalWards()
        {
            var query = "SELECT COUNT(*) FROM Ward WHERE InActive = 'N'"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        public async Task<double> GetAverageBedsPerWard()
        {
            var query = @"
                SELECT AVG(BedCount) 
                FROM (
                        SELECT COUNT(*) AS BedCount 
                        FROM Bed 
                        WHERE InActive = 'N'
                        GROUP BY WardId 
                     ) AS BedCounts"; // Alias the subquery
            return await _dbConnection.ExecuteScalarAsync<double>(query);
        }

        public async Task<List<ConditionStats>> GetConditionsStats()
        {
            var query = "SELECT Conditions, COUNT(*) AS Count FROM Condition WHERE InActive = 'N' GROUP BY Conditions";
            var result = await _dbConnection.QueryAsync<ConditionStats>(query);
            return result.ToList();
        }

        public async Task<int> GetTotalActiveUsers()
        {
            var query = "SELECT COUNT(*) FROM Users WHERE InActive = 'N'"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        public async Task<int> GetTotalInactiveUsers()
        {
            var query = "SELECT COUNT(*) FROM Users WHERE InActive = 'Y'"; // Adjust based on your schema
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }
    }

}
