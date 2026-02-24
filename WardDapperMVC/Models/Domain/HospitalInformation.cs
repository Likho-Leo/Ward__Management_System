using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WardDapperMVC.Models.Domain
{
    public class HospitalInformation
    {
        [Key]
        public int InfoID { get; set; }

        [Required(ErrorMessage = "Hospital Name is required.")]
        [StringLength(100, ErrorMessage = "Hospital Name cannot exceed 100 characters.")]
        public string HospitalName { get; set; }

        [NotMapped]
        public IFormFile? LogoFile { get; set; } //This will help is uploading the logo photo that will be change to byte format
        public byte[]? Logo { get; set; }

        [Required(ErrorMessage = "Slogan is required.")]
        [StringLength(100, ErrorMessage = "Slogan cannot exceed 100 characters.")]
        public string Slogan { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(20, ErrorMessage = "Phone Number cannot exceed 20 characters.")]
        public string TellNO { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }
    }
}
