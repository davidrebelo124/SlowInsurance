
namespace SlowInsurance.Models.File
{
    public class FileInvoiceModel
    {
        public string? PaymentType { get; set; }
        public double Value { get; set; }
        public string? ExpirationDate { get; set; }
        public string? IssuedDate { get; set; }
    }
}
