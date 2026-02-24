using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WardDapperMVC.Models.Domain
{
    public class Bed
    {
        [Key]
        public int BedID { get; set; }

        [Required(ErrorMessage = "Bed Number is required.")]
        public int BedNo { get; set; }

        [Required(ErrorMessage = "Bed Availability Status is required.")]
        public string BedAvailabilityStatus { get; set; }

        //This is for working with ward as a foreignKey and will help us to show wardName in the dropdown
        [ForeignKey("WardId")]
        [Required(ErrorMessage = "Ward is required.")]
        public int? WardId { get; set; } // Foreign key
        public string? WardName { get; set; }
    }
}
