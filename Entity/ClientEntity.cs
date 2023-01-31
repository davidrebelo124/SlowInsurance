using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    public class ClientEntity : IdentityUser
    {
        public ClientEntity()
        {
            Vehicles = new List<VehicleEntity>();
        }

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? NIF { get; set; }
        [Required]
        public string? IBAN { get; set; }
        [Required]
        public string? Birthday { get; set; }
        [Required]
        public string? Historic { get; set; }

        public virtual ICollection<VehicleEntity> Vehicles { get; set; }
    }
}
