using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class PatientMovement
    {
        [Key]
        public int MovementID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Status { get; set; }

        //This is for working with Patient Details 
        [ForeignKey("PatientId")]
        public int PatientId { get; set; } // Foreign key
        // Patient details
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
       
        public string? PatientNumber { get; set; }

        //This is for Ward with only ward details we want
        [ForeignKey("WardId")]
        public int WardId { get; set; } // Foreign key

        //ward detail that will be shown in the dropdown instead of WardID
        public string? WardName { get; set; }

        //This is for Bed only bed details we want
        [ForeignKey("BedID")]
        public int BedID { get; set; } // Foreign key

        //bed details
        public int? PreviousBedID { get; set; } // Add this to keep track of the previous bed
        public int? BedNo { get; set; }
    }
}
