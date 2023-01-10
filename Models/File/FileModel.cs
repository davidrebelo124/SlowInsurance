
namespace SlowInsurance.Models.File
{
    public class FileModel
    {
        public FileUserModel User { get; set; }
        public List<FileVehicleModel>? Vehicles { get; set; }
    }
}
