using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class DischargePatient
    {
        [Key]
        public int DischargeID { get; set; }
        [Required]
        public DateTime DischargeDate { get; set; }

        //This is for patient with Only Details that we want
        [ForeignKey("FolderID")]
        public int FolderID { get; set; } // Foreign key
    }
}
