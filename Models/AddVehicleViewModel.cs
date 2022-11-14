using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models
{
    public class AddVehicleViewModel
    {
        [Required]
        [Display(Name = "Vehicle Type")]
        public string? VehicleType { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        [Display(Name = "Registration Date")]
        public string? RegistrationDate { get; set; }
        [Required]
        public string? Plate { get; set; }
        [Required]
        [Display(Name = "Adhesion Date")]
        public string? AdhesionDate { get; set; }
        [Required]
        [Display(Name = "Payment Schedule")]
        public string? PaymentSchedule { get; set; }
    }
}
