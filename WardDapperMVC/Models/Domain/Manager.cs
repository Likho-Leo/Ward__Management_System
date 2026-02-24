using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class Manager
    {
        [Key]
        public int ManagerID { get; set; }
        [ForeignKey("UserID")]
        public int UserID { get; set; }  // Foreign key to the User table
        public string Role { get; set; }
    }
}
