
namespace SlowInsurance.Models.File
{
    public class FileInvoiceModel
    {
        public string? VehiclePlate { get; set; }
        public string? VehicleModel { get; set; }
        public string? PaymentType { get; set; }
        public double Value { get; set; }
        public string? ExpirationDate { get; set; }
        public string? IssuedDate { get; set; }
        public bool IsRenewalNeeded { get => ExpirationDate != null && DateTime.ParseExact(ExpirationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.DefaultThreadCurrentCulture) < DateTime.Now; }
    }
}
