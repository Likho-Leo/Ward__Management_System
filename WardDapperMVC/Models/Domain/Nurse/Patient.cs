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
    public class Patient
    {
        public int PatientId { get; set; }
        [Required]
        public int PatientNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateOnly DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string IDNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Addresss { get; set; }
        [Required]
        public string status { get; set; }
        [ForeignKey("ConditionID")]
        public int ConditionID { get; set; }
        [ForeignKey("AllergyID")]
        public int AllergyID { get; set; }
        [ForeignKey("MedID")]
        public int MedicationID { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string MedicalHistory { get; set; }



    }
}
