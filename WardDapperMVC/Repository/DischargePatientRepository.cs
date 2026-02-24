using Dapper;
using System.Data;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class DischargePatientRepository: IDischargePatientRepository
    {
        private readonly IDbConnection _dbConnection;

        public DischargePatientRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddDischargeRecordAsync(DischargePatient discharge)
        {
            var query = "INSERT INTO DischargePatient (FolderID, DischargeDate) VALUES (@FolderID, @DischargeDate)";
            var parameters = new { discharge.FolderID, discharge.DischargeDate };
            await _dbConnection.ExecuteAsync(query, parameters);
        }
    }
}
