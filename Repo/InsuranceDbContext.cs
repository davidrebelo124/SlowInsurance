using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlowInsurance.Entity;

namespace SlowInsurance.Repo
{
    public class InsuranceDbContext : IdentityDbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : base(options)
        {
        }

        public DbSet<ClientEntity> Client { get; set; }
        public DbSet<VehicleEntity> Vehicle { get; set; }
        public DbSet<PaymentEntity> Payment { get; set; }
        public DbSet<AccidentEntity> Accident { get; set; }

    }
}
