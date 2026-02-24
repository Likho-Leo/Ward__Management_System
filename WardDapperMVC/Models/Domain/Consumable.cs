using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class Consumable
    {
        [Key]
        public int ConsumableID { get; set; }

        [Required(ErrorMessage = "ConsumableType is required.")]
        public string ConsumableType { get; set; }

        [Required(ErrorMessage = "StockOnHand is required.")]
        public int StockOnHand { get; set; }

        [Required(ErrorMessage = "ParLevel is required.")]
        public int ParLevel { get; set; }
        [Required(ErrorMessage = "ReOrderPoint is required.")]
        public int ReorderPoint { get; set; }

        //This is for working with Manager as a foreignKey and will help us to search using EmployeeNumber and auto fill FistName and LastName
        [ForeignKey("ManagerID")]
        [Required(ErrorMessage = "Manager is required.")]
        public int ManagerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeNumber { get; set; } //Used For searching employee or user details
    }
}
