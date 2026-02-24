using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class Ward
    {
        [Key]
        public int WardID { get; set; }

        [Required(ErrorMessage = "Ward Type is required.")]
        [StringLength(100)]
        public string WardType { get; set; }

        [Required(ErrorMessage = "Ward Name is required.")]
        [StringLength(100)]
        public string WardName { get; set; }

        [Required(ErrorMessage = "Total Beds is required.")]
        public int TotalBeds { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50)]
        public string Status { get; set; }
    }
}
