using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Vehicle
{
    public class AddVehicleModel
    {
        [Required]
        [EnumDataType(typeof(VehicleType), ErrorMessage = "Not a valid type")]
        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Model { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }
        [Required]
        [RegularExpression(@"^(([A-Z]{2}-\d{2}-(\d{2}|[A-Z]{2}))|(\d{2}-(\d{2}-[A-Z]{2}|[A-Z]{2}-\d{2})))$", ErrorMessage = "Not a valid plate")]
        public string? Plate { get; set; }
    }
}
