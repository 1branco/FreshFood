using Database.Entities.Customer;
using Database.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts
{
    public class CoreDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerBillingInfo> CustomersBillingInfo { get; set; }
        public DbSet<Credential> CustomersCredentials { get; set; }

        public CoreDbContext(DbContextOptions<CoreDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {

        }
        public CoreDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 1 Customer could own N credentials
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Credentials)
                .WithOne()
                .IsRequired();

            #endregion
        }
    }    

}
