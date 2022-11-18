using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models
{
    public class ForgotYourPasswordFormViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
