using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    public class InvoiceEntity
    {
        public int Id { get; set; }
        [Required]
        public string? PaymentType { get; set; }
        [Required]
        public float Value { get; set; }
        [Required]
        public string? Val { get; set; }
    }
}
