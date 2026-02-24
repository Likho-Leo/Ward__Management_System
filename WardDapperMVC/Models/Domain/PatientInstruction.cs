using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WardDapperMVC.Models.Domain
{
    public class PatientInstruction
    {
        public int InstructionID { get; set; }

        [ForeignKey("PatientID")]
        [DisplayName("Patient")]
        public int PatientID { get; set; }

        [ForeignKey("UserID")]
        [DisplayName("User")]
        public int DoctorID { get; set; }

        [DisplayName("Date of Visit")]
        public DateTime DateOfVisit { get; set; }

        [DisplayName("Follow Up Appointment Date")]
        public DateTime? FollowUpAppointmentDate { get; set; } // Nullable, since it's checked

        [DisplayName("Doctor Note")]
        public string? DoctorNote { get; set; }

        [DisplayName("Discharge Status")]
        public string? DischargeStatus { get; set; }

        [DisplayName("Rest")]
        public string? Rest { get; set; }

        [DisplayName("Wound Care")]
        public string? WoundCare { get; set; }

        [DisplayName("Medications")]
        public string? Medications { get; set; }

        [DisplayName("Diet")]
        public string? Diet { get; set; }

        [DisplayName("Emergency Signs")]
        public string? EmergencySigns { get; set; }

        public string? PatientFullName { get; set; }
        public string? UserFullName { get; set; }
    }
}
