using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    public class AccidentEntity
    {
        public AccidentEntity()
        {
            Vehicles = new List<VehicleEntity>();
        }

        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public string? Date { get; set; }

        public virtual ICollection<VehicleEntity> Vehicles { get; set; }
    }
}
