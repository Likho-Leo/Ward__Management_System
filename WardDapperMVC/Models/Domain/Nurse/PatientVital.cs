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
    public class PatientVital
    {
        [DisplayName("Vital InstructionID")]
        public int VitalId { get; set; }
        [Required(ErrorMessage = "Blood Pressure is required.")]
        [RegularExpression(@"^\d{1,3}/\d{1,3}$", ErrorMessage = "Blood Pressure must be in the format '120/80'.")]
        [DisplayName("Blood Pressure (mmHg)")]
        public string BloodPressure { get; set; } // VARCHAR in SQL

        [Required(ErrorMessage = "Blood Oxygen Level is required.")]
        [Range(0, 100, ErrorMessage = "Blood Oxygen Level must be between 0 and 100.")]
        [DisplayName("Blood Oxygen Level (SpO2)")]
        public int BloodOxygen { get; set; } // TINYINT in SQL

        [Required(ErrorMessage = "Blood Glucose Level is required.")]
        [Range(0, 600, ErrorMessage = "Blood Glucose Level must be between 0 and 600 mg/dl.")]
        [DisplayName("Blood Glucose Level (mg/dl)")]
        public decimal BloodGlucoseLvl { get; set; } // DECIMAL(5,2) in SQL

        [Required(ErrorMessage = "Pulse Rate is required.")]
        [Range(0, 200, ErrorMessage = "Pulse Rate must be between 0 and 200 bpm.")]
        [DisplayName("Pulse Rate (bpm)")]
        public int PulseRate { get; set; } // TINYINT in SQL

        [Required(ErrorMessage = "Respiration Rate is required.")]
        [Range(0, 60, ErrorMessage = "Respiration Rate must be between 0 and 60 bpm.")]
        [DisplayName("Respiration Rate (bpm)")]
        public int RespirationRate { get; set; } // TINYINT in SQL

        [Required(ErrorMessage = "Body Temperature is required.")]
        [Range(28, 45, ErrorMessage = "Body Temperature must be between 28 and 45 °C.")]
        [DisplayName("Body Temperature (°C)")]
        public decimal BodyTemp { get; set; } // DECIMAL(4,2) in SQL

        [Required(ErrorMessage = "Body Weight is required.")]
        [Range(0, 500, ErrorMessage = "Body Weight must be between 0 and 500 kg.")]
        [DisplayName("Body Weight (kg)")]
        public decimal Weight { get; set; } // DECIMAL(5,2) in SQL
        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("UserID")]
        public int UserId { get; set; }
        [DisplayName("Employee Number")]
        public string? EmployeeNumber { get; set; }
        [DisplayName("Nurse First Name")]
        public string? NurseFirstName { get; set; }
        [DisplayName("Nurse Last Name")]
        public string? NurseLastName { get; set; }

        [ForeignKey("PatientID")]
        public int PatientId { get; set; }
        [DisplayName("Patient Number")]
        public string? PatientNumber { get; set; }
        [DisplayName("Patient First Name")]
        public string? PatientFirstName { get; set; }
        [DisplayName("Patient Last Name")]
        public string? PatientLastName { get; set; }

    }
}
