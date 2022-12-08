using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
