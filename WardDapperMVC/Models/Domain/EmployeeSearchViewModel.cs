
namespace WardDapperMVC.Models.Domain
{
    public class EmployeeSearchViewModel
    {
        public string? Role { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<User> Employees { get; set; } = new List<User>();
    }
}
