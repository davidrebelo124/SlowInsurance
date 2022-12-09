using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Vehicle
{
    public class AddVehicleModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Vehicle Type")]
        public string? VehicleType { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Model { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Registration Date")]
        public string? RegistrationDate { get; set; }
        [Required]
        [RegularExpression("/^(([A-Z]{2}-\\d{2}-(\\d{2}|[A-Z]{2}))|(\\d{2}-(\\d{2}-[A-Z]{2}|[A-Z]{2}-\\d{2})))$/")]
        public string? Plate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Adhesion Date")]
        public string? AdhesionDate { get; set; }
    }
}
