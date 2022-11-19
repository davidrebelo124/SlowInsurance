using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlowInsurance.Entity;

namespace SlowInsurance.Repo
{
    public class InsuranceDbContext : IdentityDbContext<ClientEntity>
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : base(options)
        {
        }

        public DbSet<VehicleEntity> Vehicle { get; set; }
        public DbSet<InvoiceEntity> Invoice { get; set; }
        public DbSet<AccidentEntity> Accident { get; set; }

    }
}
