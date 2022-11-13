using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? NIF { get; set; }
        [Required]
        public string? IBAN { get; set; }
        [Required]
        public string? Birthday { get; set; }
        [Required]
        public string? Historic { get; set; }
        [Required]
        [Display(Name = "Driver's License Number")]
        public string? DriverLicense { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", 
            ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
