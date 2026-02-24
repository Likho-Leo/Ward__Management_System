using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class PatientSearchViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today; // Default to today

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today; // Default to today


        public List<PatientFolder> Patients { get; set; } = new List<PatientFolder>();
        public HospitalInformation? HospitalInformation { get; set; }
    }
}
