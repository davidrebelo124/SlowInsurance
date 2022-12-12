using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Invoice
{
    public class InvoiceModel
    {
        [Required]
        [EnumDataType(typeof(PaymentType), ErrorMessage = "Invalid payment type")]
        public PaymentType PaymentType { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public DateTime? IssuedDate { get; set; }
        [Required]
        public DateTime? ExpirationDate { get; set; }

        public bool IsRenewalNeeded { get => ExpirationDate.Value.AddMonths(-1) < DateTime.Now; }

        [Required]
        public string? Vehicle { get; set; }
    }
}
