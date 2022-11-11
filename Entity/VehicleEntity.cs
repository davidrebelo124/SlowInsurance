using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    public class VehicleEntity
    {
        public VehicleEntity()
        {
            Payments = new List<PaymentEntity>();
            Accidents = new List<AccidentEntity>();
        }

        public int Id { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        public DateOnly RegisterDate { get; set; }
        [Required]
        public string? CarPlate { get; set; }
        [Required]
        public DateOnly AdhesionDate { get; set; }
        [Required]
        public string? PaymentSchedule { get; set; }


        public virtual IList<PaymentEntity> Payments { get; set; }
        public virtual IList<AccidentEntity> Accidents { get; set; }
    }
}
