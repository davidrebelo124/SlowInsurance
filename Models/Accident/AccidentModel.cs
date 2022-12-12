using SlowInsurance.Models.Vehicle;
using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Accident
{
    public class AccidentModel
    {
        [Required]
        [MaxLength(100)]
        public string? Location { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        [Required]
        public List<VehicleModel>? Vehicles { get; set; }
    }
}
