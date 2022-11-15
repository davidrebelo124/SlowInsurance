using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public string? VehicleType { get; set; }
        public string? Model { get; set; }
        [Display(Name = "Registration Dates")]
        public string? RegistrationDate { get; set; }
        public string? Plate { get; set; }
        [Display(Name = "Adhesion Date")]
        public string? AdhesionDate { get; set; }
    }
}
