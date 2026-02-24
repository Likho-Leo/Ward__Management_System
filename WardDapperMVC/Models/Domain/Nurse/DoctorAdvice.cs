using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WardDapperMVC.Models.Domain.Nurse
{
    public class DoctorAdvice
    {
        [Key]
        public int InstructionID { get; set; }
        [Required]
        [DisplayName("Doctor's Note")]
        public string DoctorNote { get; set; }
        [Required]
        [DisplayName("Discharge Status")]
        public string DischargeStatus { get; set; }
        [Required]
        [DisplayName("Date of Visit")]
        public DateTime DateOfVisit { get; set; }
        [Required]
        [DisplayName("Follow up Appointment Date")]
        public DateTime? FollowUpAppointmentDate {  get; set; }
        [Required]
        public string Rest {  get; set; }
        [Required]
        [DisplayName("Wound Care")]
        public string WoundCare {  get; set; }
        [Required]
        [DisplayName("Medications")]
        public string Medications { get; set; }
        [Required]
        public string Diet {  get; set; }
        [Required]
        [DisplayName("Emergency Signs")]
        public string EmergencySigns { get; set; }

        [ForeignKey("PatientID")]
        public int PatientID { get; set; }
        [DisplayName("Patient Number")]
        public string? PatientNumber { get; set; }
        [DisplayName("Patient First Name")]
        public string? PatientFirstName { get; set; }
        [DisplayName("Patient Last Name")]
        public string? PatientLastName { get; set; }

        [ForeignKey("UserID")]
        public int UserID { get; set; }
        [DisplayName("Employee Number")]
        public string? EmployeeNumber { get; set; }
        [DisplayName("Doctor First Name")]
        public string? DoctorFirstName { get; set; }
        [DisplayName("Doctor Last name")]
        public string? DoctorLastName { get; set; }

        [ForeignKey("DoctorID")]
        public int DoctorID { get; set; }
    }
}
