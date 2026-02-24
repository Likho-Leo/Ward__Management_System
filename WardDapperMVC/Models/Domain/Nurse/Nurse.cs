using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WardDapperMVC.Models.Domain.Nurse
{
    public class Nurse
    {
        public int Id { get; set; }
        [ForeignKey("UserID"), DisplayName("VitalId")]
        public int UserId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
