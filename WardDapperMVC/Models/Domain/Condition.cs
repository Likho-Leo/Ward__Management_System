using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class Condition
    {
        [Key]
        public int ConditionID { get; set; }

        [Required(ErrorMessage = "Condition is required.")]
        [StringLength(100)]
        public string Conditions { get; set; }

        [Required(ErrorMessage = "Condition Type is required.")]
        [StringLength(100)]
        public string ConditionType { get; set; }
    }
}
