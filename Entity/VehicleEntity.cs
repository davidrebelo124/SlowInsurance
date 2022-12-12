using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    [Index(nameof(Plate), IsUnique = true)]
    public class VehicleEntity
    {
        public VehicleEntity()
        {
            Invoices = new List<InvoiceEntity>();
            Accidents = new List<AccidentEntity>();
        }

        public int Id { get; set; }
        [Required]
        public string? VehicleType { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        public string? RegistrationDate { get; set; }
        [Required]
        public string? Plate { get; set; }
        [Required]
        public string? AdhesionDate { get; set; }


        public virtual ICollection<InvoiceEntity> Invoices { get; set; }
        public virtual ICollection<AccidentEntity> Accidents { get; set; }
    }
}
