using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
