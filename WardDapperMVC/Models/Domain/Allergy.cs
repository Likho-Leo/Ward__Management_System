using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class Allergy
    {
        [Key]
        public int AllergyID { get; set; }
        [Required(ErrorMessage = "Allergen is required.")]
        [StringLength(100)]
        public string Allergen { get; set; }

        [Required(ErrorMessage = "Allergy Type is required.")]
        [StringLength(100)]
        public string AllergyType { get; set; }

        [Required(ErrorMessage = "Symptoms is required.")]
        [StringLength(100)]
        public string Symptoms { get; set; }
    }
}
