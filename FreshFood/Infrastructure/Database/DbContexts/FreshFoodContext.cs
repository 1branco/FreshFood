using Database.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.DbContexts
{
    public class FreshFoodContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<UserBillingInfo> UsersBillingInfo { get; set; }
        public DbSet<UserShippingInfo> UsersShippingInfo { get; set; }

        public FreshFoodContext(DbContextOptions<FreshFoodContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
        public FreshFoodContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
