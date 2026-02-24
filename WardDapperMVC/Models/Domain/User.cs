using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WardDapperMVC.Models.Domain
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string? EmployeeNumber { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(10)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10)] 
        public string Gender { get; set; }

        [Required(ErrorMessage = "ID Number is required.")]
        [StringLength(13)]
        [IDAndDOBMatch(ErrorMessage = "ID Number does not match Date of Birth.")]
        public string IDNo { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        //New
        [Required(ErrorMessage = "Address1 is required.")]
        [StringLength(252)] 
        public string AddressLine1 { get; set; }
       
        [StringLength(255)]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "PostalCode is required.")]
        [StringLength(255)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        [StringLength(255)]
        public string Province { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(255)]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(255)]
        public string City { get; set; }

        [Required(ErrorMessage = "Town/Surburb is required.")]
        [StringLength(255)]
        public string Town_Surburb { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50)] 
        public string Role { get; set; }

        [Required(ErrorMessage = "EmploymentDate is required.")]
        public DateTime EmploymentDate { get; set; }

        public class IDAndDOBMatchAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var user = (User)validationContext.ObjectInstance;

                if (user.DOB == null || string.IsNullOrEmpty(user.IDNo))
                {
                    return new ValidationResult(ErrorMessage);
                }

                if (!IsIDAndDOBValid(user.IDNo, user.DOB))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            private static bool IsIDAndDOBValid(string idNumber, DateTime dob)
            {
                // Example validation logic: Check if ID number starts with the birth year (last 2 digits)
                string birthYear = dob.Year.ToString().Substring(2, 2);
                return idNumber.StartsWith(birthYear);
            }
        }
    }
}
