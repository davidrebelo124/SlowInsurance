using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Entity
{
    public class InvoiceEntity
    {
        public int Id { get; set; }
        [Required]
        public string? PaymentType { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public string? ExpirationDate { get; set; }
        [Required]
        public string? IssuedDate { get; set; }
    }
}
