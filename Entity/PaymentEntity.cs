using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    public class PaymentEntity
    {
        public int Id { get; set; }
        [Required]
        public string? PaymentType { get; set; }
        [Required]
        public float Value { get; set; }
        [Required]
        public string? Validity { get; set; }
    }
}
