using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class Medication
    {
        [Key]
        public int MedID { get; set; }

        [Required(ErrorMessage = "Medication Type is required.")]
        [StringLength(100)]
        public string MedicationType { get; set; }

        [Required(ErrorMessage = "Medication Name is required.")]
        [StringLength(100)]
        public string MedicationName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
