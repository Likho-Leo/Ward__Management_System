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
    public class MedAdministration
    {
        public int AdminMedID { get; set; }
        [Required(ErrorMessage ="Medication is required.")]
        [DisplayName("Medication")]
        public string Med { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime Date { get; set; }
        [Required]
        public string? status { get; set; }

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
