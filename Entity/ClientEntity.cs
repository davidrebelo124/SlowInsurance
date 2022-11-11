namespace SlowInsurance.Entity
{
    public class ClientEntity
    {
        public ClientEntity()
        {
            Vehicles = new List<VehicleEntity>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? NIF { get; set; }
        public string? IBAN { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateOnly Birthday { get; set; }
        public string? Historic { get; set; }
        public string? DriverLicense { get; set; }

        public IList<VehicleEntity> Vehicles { get; set; }
    }
}
