
namespace SlowInsurance.Models.File
{
    public class FileVehicleModel
    {
        public string? Plate { get; set; }
        public string? Model { get; set; }
        public List<FileInvoiceModel>? Invoices { get; set; }
    }
}
