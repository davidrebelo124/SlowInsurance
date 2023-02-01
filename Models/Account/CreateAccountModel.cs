using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Account
{
    public class CreateAccountModel
    {
        [Required]
        [MaxLength(70)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
