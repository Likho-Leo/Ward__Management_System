
namespace WardDapperMVC.Models.Domain
{
    public class DashboardViewModel
    {
        public int TotalHospitals { get; set; }
        public int TotalBeds { get; set; }
        public int TotalConditions { get; set; }
        public int TotalConsumables { get; set; }
        public int TotalMedications { get; set; }
        public int TotalUsers { get; set; }
        public int TotalWards { get; set; }

        public double AverageBedsPerWard { get; set; }
        public List<ConditionStats> Conditions { get; set; }

        public int TotalActiveUsers { get; set; }
        public int TotalInactiveUsers { get; set; }
    }

    public class ConditionStats
    {
        public string Conditions { get; set; }
        public int Count { get; set; }
    }
}
