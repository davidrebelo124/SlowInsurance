using SlowInsurance.Entity;
using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Accident
{
    public class AddAccidentModel
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
        public string? Vehicles { get; set; }
    }
}
