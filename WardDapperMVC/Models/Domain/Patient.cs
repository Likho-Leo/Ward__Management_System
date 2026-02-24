using System.ComponentModel.DataAnnotations;

namespace WardDapperMVC.Models.Domain
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        public string? PatientNumber { get; set; }

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

        [Required(ErrorMessage = "Date Of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "ID Number is required.")]
        [StringLength(13)]
        [IDNOAndDOBMatchAttribute(ErrorMessage = "ID Number does not match Date of Birth.")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        // Address fields
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

        // Medical details
        public string? MedicalHistory { get; set; }
        public string? Allergies { get; set; }
        public string? CurrentMedications { get; set; }

        // Next of Kin properties
        [Required(ErrorMessage = "Next Of Kin FullName is required.")]
        [StringLength(50)]
        public string NextOfKinName { get; set; }

        [Required(ErrorMessage = "Next Of Kin Relationship is required.")]
        [StringLength(50)]
        public string NextOfKinRelationship { get; set; }

        [Required(ErrorMessage = "Next Of Kin PhoneNumber is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(20)]
        public string NextOfKinPhoneNumber { get; set; }

        // New Emergency Contact fields
        [StringLength(50)]
        public string? EmergencyContactName { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(20)]
        public string? EmergencyContactPhone { get; set; }

        // New Insurance Information fields
        [StringLength(100)]
        public string? InsuranceProvider { get; set; }

        [StringLength(50)]
        public string? PolicyNumber { get; set; }

        // Refer Patient ID
        public int? ReferPatientID { get; set; }

        public class IDNOAndDOBMatchAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var patient = (Patient)validationContext.ObjectInstance;

                if (patient.DOB == null || string.IsNullOrEmpty(patient.IDNumber))
                {
                    return new ValidationResult(ErrorMessage);
                }

                if (!IsIDAndDOBValid(patient.IDNumber, patient.DOB))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            private static bool IsIDAndDOBValid(string idNumber, DateTime dob)
            {
                string birthYear = dob.Year.ToString().Substring(2, 2);
                return idNumber.StartsWith(birthYear);
            }
        }
    }
}
