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
    public class TreatPatient
    {
        public int TreatPatientID { get; set; }
        [Required]
        public string TreatmentType { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("PatientID")]
        public int PatientId { get; set; }
        public string? PatientNumber { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }

        [ForeignKey("UserID")]
        public int UserId { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? NurseFirstName { get; set; }
        public string? NurseLastName { get; set; }
    }
}
