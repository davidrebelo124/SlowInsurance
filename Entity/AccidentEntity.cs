using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    public class AccidentEntity
    {
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Location { get; set; }
    }
}
