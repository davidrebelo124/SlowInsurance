using System.ComponentModel.DataAnnotations;

namespace SlowInsurance.Models.Invoice
{
    public class RenewInvoiceModel
    {
        [Required]
        [EnumDataType(typeof(PaymentType), ErrorMessage = "Invalid payment type")]
        public PaymentType PaymentType { get; set; }
        [Required]
        public double Value { get; set; }

        [Required]
        public string? Plate { get; set; }
        [Required]
        public string? Model { get; set; }
    }
}
