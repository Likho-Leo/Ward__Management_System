using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class PatientReferrals
    {
        [Key]
        public int ReferPatientID { get; set; }

        public string? EmployeeNumber { get; set; }

        [Required]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Status { get; set; }
        
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
    }
}
