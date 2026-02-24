using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Additional fields for extended login functionality
        public bool RememberMe { get; set; } // Add this property
    }
}

