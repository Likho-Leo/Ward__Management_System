using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardDapperMVC.Models.Domain.Nurse
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public int EmployeeNo { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string IdNumber { get; set; }
        [Required]
        public DateOnly DOB { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
